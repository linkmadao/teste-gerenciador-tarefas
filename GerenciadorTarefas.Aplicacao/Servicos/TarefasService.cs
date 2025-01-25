using AutoMapper;
using GerenciadorTarefas.Aplicacao.Dtos;
using GerenciadorTarefas.Aplicacao.Interfaces;
using GerenciadorTarefas.Dominio.Constantes;
using GerenciadorTarefas.Dominio.Entidades;
using GerenciadorTarefas.Dominio.Enumeradores;
using GerenciadorTarefas.Dominio.Excecoes;
using GerenciadorTarefas.Dominio.Interfaces;
using Microsoft.Extensions.Logging;

namespace GerenciadorTarefas.Aplicacao.Servicos;

public class TarefasService : ITarefasService
{
    private readonly ILogger<TarefasService> _logger;
    private readonly IMapper _mapper;
    private readonly ITarefasRepository _tarefasRepository;
    private readonly IHistoricosTarefasService _historicosTarefasService;

    public TarefasService(
        ILogger<TarefasService> logger, 
        IMapper mapper, 
        ITarefasRepository tarefasRepository,
        IHistoricosTarefasService historicosTarefasService)
    {
        _logger = logger;
        _mapper = mapper;
        _tarefasRepository = tarefasRepository;
        _historicosTarefasService = historicosTarefasService;
    }

    public async Task<PaginacaoDto<TarefaDto>> ListarTarefas(int projetoId, int numeroPagina)
    {
        ValidarProjetoId(projetoId);

        var totalDeTarefasPorProjetos = await _tarefasRepository.TotalDeTarefasPorProjetoAsync(projetoId);
        if (totalDeTarefasPorProjetos == 0)
            return new PaginacaoDto<TarefaDto>
            {
                Pagina = numeroPagina,
                Dados = []
            };

        var listaTarefas = await _tarefasRepository.ListarTarefasAsync(projetoId, numeroPagina, 10);
        var listaTarefasDto = _mapper.Map<List<TarefaDto>>(listaTarefas);

        return new PaginacaoDto<TarefaDto>
        {
            Pagina = numeroPagina,
            Dados = listaTarefasDto
        };
    }

    private void ValidarProjetoId(int projetoId)
    {
        if (projetoId < 0)
        {
            var mensagemErro = "ProjetoId não pode ser menor que 0";
            _logger.LogWarning("{MensagemRegraNegocio} {MensagemErro}", MensagensRegraNegocio.ErroRegraNegocio, mensagemErro);
            throw new RegraDeNegocioException(mensagemErro);
        }
    }

    public async Task Criar(TarefaDto tarefaDto)
    {
        ValidarProjetoId(tarefaDto.ProjetoId);

        var totalDeTarefasNoProjeto = await _tarefasRepository.TotalDeTarefasNaoConcluidasPorProjetoAsync(tarefaDto.ProjetoId);
        if (totalDeTarefasNoProjeto >= 20)
        {
            var mensagemErro = "Já existem 20 tarefas cadastradas no projeto. Caso queira cadastrar mais tente remover uma.";
            _logger.LogWarning("{MensagemRegraNegocio} {MensagemErro}", MensagensRegraNegocio.ErroRegraNegocio, mensagemErro);
            throw new RegraDeNegocioException(mensagemErro);
        }

        var tarefa = _mapper.Map<Tarefa>(tarefaDto);
        await _tarefasRepository.CriarAsync(tarefa);

        await _historicosTarefasService.CriarHistorico(tarefa, OperacaoRealizada.Inclusao);
    }

    public async Task Atualizar(TarefaDto tarefaDto)
    {
        var tarefa = await _tarefasRepository.ObterAsync(tarefaDto.Id);
        ValidarTarefa(tarefa);
        ValidarPrioridadeTarefa(tarefa, tarefaDto);

        await _historicosTarefasService.CriarHistorico(tarefa, OperacaoRealizada.Alteracao);

        AtualizarTarefa(tarefa, tarefaDto);
        await _tarefasRepository.AtualizarAsync(tarefa);
    }

    private void ValidarTarefa(Tarefa tarefa)
    {
        if (tarefa is null)
        {
            var mensagemErro = "Tarefa não encontrada";
            _logger.LogWarning("{MensagemRegraNegocio} {MensagemErro}", MensagensRegraNegocio.ErroRegraNegocio, mensagemErro);
            throw new RegraDeNegocioException(mensagemErro);
        }
    }

    private void ValidarPrioridadeTarefa(Tarefa tarefa, TarefaDto tarefaDto)
    {
        if (tarefa.Prioridade != tarefaDto.Prioridade)
        {
            var mensagemErro = "Não é possível alterar a prioridade da tarefa!";
            _logger.LogWarning("{MensagemRegraNegocio} {MensagemErro}", MensagensRegraNegocio.ErroRegraNegocio, mensagemErro);
            throw new RegraDeNegocioException(mensagemErro);
        }
    }

    private void AtualizarTarefa(Tarefa tarefa, TarefaDto tarefaDto)
    {
        tarefa.AtualizarDescricao(tarefaDto.Descricao);
        tarefa.AtualizarStatus(tarefaDto.Status);

        var comentariosAdicionados = tarefaDto.Comentarios
            .ExceptBy(tarefa.Comentarios.Select(comentario => comentario.Id), comentarioDto => comentarioDto.Id)
            .Select(_mapper.Map<Comentario>)
            .ToList();

        tarefa.AdicionarComentarios(comentariosAdicionados);
    }

    public async Task Remover(int tarefaId)
    {
        var tarefa = await _tarefasRepository.ObterAsync(tarefaId);
        ValidarTarefa(tarefa);

        await _historicosTarefasService.CriarHistorico(tarefa, OperacaoRealizada.Exclusao);

        tarefa.SetarComoDeletado();
        await _tarefasRepository.RemoverAsync(tarefa);
    }
}
