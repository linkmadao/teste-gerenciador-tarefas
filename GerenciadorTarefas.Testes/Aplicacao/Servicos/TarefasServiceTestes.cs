using AutoMapper;
using GerenciadorTarefas.Aplicacao.Dtos;
using GerenciadorTarefas.Aplicacao.Interfaces;
using GerenciadorTarefas.Aplicacao.Servicos;
using GerenciadorTarefas.Dominio.Entidades;
using GerenciadorTarefas.Dominio.Enumeradores;
using GerenciadorTarefas.Dominio.Excecoes;
using GerenciadorTarefas.Dominio.Interfaces;
using GerenciadorTarefas.Testes.Traits;
using Microsoft.Extensions.Logging;
using Moq;

namespace GerenciadorTarefas.Testes.Aplicacao.Servicos;

public class TarefasServiceTestes
{
    private readonly Mock<ILogger<TarefasService>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ITarefasRepository> _mockTarefasRepository;
    private readonly Mock<IHistoricosTarefasService> _mockHistoricosTarefasService;
    private readonly TarefasService _service;

    public TarefasServiceTestes()
    {
        _mockLogger = new Mock<ILogger<TarefasService>>();
        _mockMapper = new Mock<IMapper>();
        _mockTarefasRepository = new Mock<ITarefasRepository>();
        _mockHistoricosTarefasService = new Mock<IHistoricosTarefasService>();

        _service = new TarefasService(
            _mockLogger.Object,
            _mockMapper.Object,
            _mockTarefasRepository.Object,
            _mockHistoricosTarefasService.Object);
    }

    #region ListarTarefas
    [Fact]
    public async Task ListarTarefas_RegraDeNegocioException_QuandoValidarProjetoId()
    {
        // Arrange
        var projetoIdInvalido = -1;
        var numeroPagina = 1;

        // Act
        var resultadoExecucao = () => _service.ListarTarefas(projetoIdInvalido, numeroPagina);

        // Assert
        await Assert.ThrowsAsync<RegraDeNegocioException>(resultadoExecucao);
    }

    [Fact]
    public async Task ListarTarefas_RegraDeNegocioException_ValidaLogWarning()
    {
        // Arrange
        var projetoIdInvalido = -1;
        var numeroPagina = 1;

        // Act
        var resultadoExecucao = () => _service.ListarTarefas(projetoIdInvalido, numeroPagina);
        var respostaBadRequest = await Assert.ThrowsAsync<RegraDeNegocioException>(resultadoExecucao);

        // Assert
        _mockLogger.Verify(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task ListarTarefas_PaginacaoDTO_ValidaListaDadosVazia()
    {
        // Arrange
        var projetoIdInvalido = 1;
        var numeroPagina = 1;

        _mockTarefasRepository
            .Setup(t => t.TotalDeTarefasPorProjetoAsync(It.IsAny<int>()))
            .ReturnsAsync(0);

        // Act
        var listaTarefas = await _service.ListarTarefas(projetoIdInvalido, numeroPagina);

        // Assert
        Assert.Equal(numeroPagina, listaTarefas.Pagina);
        Assert.Empty(listaTarefas.Dados);
    }

    [Fact]
    public async Task ListarTarefas_PaginacaoDTO_ValidaListaDadosPreenchida()
    {
        // Arrange
        var projetoIdInvalido = 1;
        var numeroPagina = 1;
        var paginacaoTarefas = TarefasTraits.PaginacaoTarefas();

        _mockTarefasRepository
            .Setup(t => t.TotalDeTarefasPorProjetoAsync(It.IsAny<int>()))
            .ReturnsAsync(paginacaoTarefas.TotalItens);

        _mockMapper.Setup(t => t.Map<List<TarefaDto>>(It.IsAny<List<Tarefa>>()))
            .Returns(paginacaoTarefas.Dados.ToList());

        _mockTarefasRepository
            .Setup(t => t.ListarTarefasAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(TarefasTraits.ListaTarefas());

        // Act
        var listaTarefas = await _service.ListarTarefas(projetoIdInvalido, numeroPagina);

        // Assert
        Assert.Equal(numeroPagina, listaTarefas.Pagina);
        Assert.NotEmpty(listaTarefas.Dados);
        Assert.Equal(paginacaoTarefas.Dados, listaTarefas.Dados);
    }
    #endregion

    #region Criar
    [Fact]
    public async Task Criar_RegraDeNegocioException_QuandoValidarProjetoId()
    {
        // Arrange
        var tarefa = TarefasTraits.TarefaDto();
        tarefa.ProjetoId = -1;

        // Act
        var resultadoExecucao = () => _service.Criar(tarefa);

        // Assert
        await Assert.ThrowsAsync<RegraDeNegocioException>(resultadoExecucao);
    }

    [Fact]
    public async Task Criar_RegraDeNegocioException_QuandoValidarTotalDeTarefasDoProjeto()
    {
        // Arrange
        var tarefa = TarefasTraits.TarefaDto();

        _mockTarefasRepository
            .Setup(t => t.TotalDeTarefasNaoConcluidasPorProjetoAsync(It.IsAny<int>()))
            .ReturnsAsync(20);

        // Act
        var resultadoExecucao = () => _service.Criar(tarefa);

        // Assert
        await Assert.ThrowsAsync<RegraDeNegocioException>(resultadoExecucao);
    }

    [Fact]
    public async Task Criar_Ok()
    {
        // Arrange
        var tarefa = TarefasTraits.TarefaDto();

        _mockTarefasRepository
            .Setup(t => t.TotalDeTarefasNaoConcluidasPorProjetoAsync(It.IsAny<int>()))
            .ReturnsAsync(19);

        _mockTarefasRepository
            .Setup(t => t.CriarAsync(It.IsAny<Tarefa>()))
            .Verifiable();

        _mockHistoricosTarefasService
            .Setup(t => t.CriarHistorico(It.IsAny<Tarefa>(), It.IsAny<OperacaoRealizada>()))
            .Verifiable();

        // Act
        await _service.Criar(tarefa);

        // Assert
        _mockTarefasRepository.Verify(t => t.CriarAsync(It.IsAny<Tarefa>()), Times.Once);
        _mockHistoricosTarefasService.Verify(t => t.CriarHistorico(It.IsAny<Tarefa>(), It.IsAny<OperacaoRealizada>()), Times.Once);
    }
    #endregion

    #region Atualizar
    [Fact]
    public async Task Atualizar_RegraDeNegocioException_QuandoNaoEncontrarTarefa()
    {
        // Arrange
        var tarefa = TarefasTraits.TarefaDto();

        _mockTarefasRepository.Setup(t => t.ObterAsync(It.IsAny<int>()));

        // Act
        var resultadoExecucao = () => _service.Atualizar(tarefa);

        // Assert
        await Assert.ThrowsAsync<RegraDeNegocioException>(resultadoExecucao);
    }

    [Fact]
    public async Task Atualizar_RegraDeNegocioException_QuandoPrioridadeForDiferenteDaOriginal()
    {
        // Arrange
        var tarefa = TarefasTraits.TarefaDto();
        tarefa.Prioridade = PrioridadeTarefa.Baixa;

        _mockTarefasRepository
            .Setup(t => t.ObterAsync(It.IsAny<int>()))
            .ReturnsAsync(TarefasTraits.Tarefa());

        // Act
        var resultadoExecucao = () => _service.Atualizar(tarefa);

        // Assert
        await Assert.ThrowsAsync<RegraDeNegocioException>(resultadoExecucao);
    }

    [Fact]
    public async Task Atualizar_Ok()
    {
        // Arrange
        var tarefa = TarefasTraits.TarefaDto();
        tarefa.Comentarios =
        [
            new ComentarioDto()
            {
                TarefaId = tarefa.Id,
                UsuarioId = tarefa.UsuarioId,
                Texto = "Comentário 1"
            }
        ];

        var tarefaExistente = TarefasTraits.Tarefa();

        _mockTarefasRepository
            .Setup(t => t.ObterAsync(It.IsAny<int>()))
            .ReturnsAsync(tarefaExistente);

        _mockHistoricosTarefasService
            .Setup(t => t.CriarHistorico(It.IsAny<Tarefa>(), It.IsAny<OperacaoRealizada>()))
            .Verifiable();

        _mockTarefasRepository
            .Setup(t => t.AtualizarAsync(It.IsAny<Tarefa>()))
            .Verifiable();

        // Act
        await _service.Atualizar(tarefa);

        // Assert
        _mockTarefasRepository.Verify(t => t.AtualizarAsync(It.IsAny<Tarefa>()), Times.Once);
        _mockHistoricosTarefasService.Verify(t => t.CriarHistorico(It.IsAny<Tarefa>(), It.IsAny<OperacaoRealizada>()), Times.Once);
    }
    #endregion

    #region Remover
    [Fact]
    public async Task Remover_RegraDeNegocioException_QuandoNaoEncontrarTarefa()
    {
        // Arrange
        var tarefaId = 1;

        _mockTarefasRepository.Setup(t => t.ObterAsync(It.IsAny<int>()));

        // Act
        var resultadoExecucao = () => _service.Remover(tarefaId);

        // Assert
        await Assert.ThrowsAsync<RegraDeNegocioException>(resultadoExecucao);
    }

    [Fact]
    public async Task Remover_Ok()
    {
        // Arrange
        var tarefaId = 1;
        var tarefaExistente = TarefasTraits.Tarefa();

        _mockTarefasRepository
            .Setup(t => t.ObterAsync(It.IsAny<int>()))
            .ReturnsAsync(tarefaExistente);

        _mockHistoricosTarefasService
            .Setup(t => t.CriarHistorico(It.IsAny<Tarefa>(), It.IsAny<OperacaoRealizada>()))
            .Verifiable();

        _mockTarefasRepository
            .Setup(t => t.RemoverAsync(It.IsAny<Tarefa>()))
            .Verifiable();

        // Act
        await _service.Remover(tarefaId);

        // Assert
        _mockTarefasRepository.Verify(t => t.RemoverAsync(It.IsAny<Tarefa>()), Times.Once);
        _mockHistoricosTarefasService.Verify(t => t.CriarHistorico(It.IsAny<Tarefa>(), It.IsAny<OperacaoRealizada>()), Times.Once);
    }
    #endregion
}