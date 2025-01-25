using GerenciadorTarefas.Api.Responses;
using GerenciadorTarefas.Aplicacao.Dtos;
using GerenciadorTarefas.Aplicacao.Interfaces;
using GerenciadorTarefas.Dominio.Constantes;
using GerenciadorTarefas.Dominio.Excecoes;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorTarefas.Api.Controllers;

[ApiController]
[Route("v1/Tarefas")]
public class TarefasController : ControllerBase
{
    private readonly ILogger<TarefasController> _logger;
    private readonly ITarefasService _tarefasService;

    public TarefasController(ILogger<TarefasController> logger, ITarefasService tarefasService)
    {
        _logger = logger;
        _tarefasService = tarefasService;
    }

    [HttpGet("ListarPorProjeto/{projetoId:int}")]
    [EndpointDescription("Lista todas as tarefas de um projeto específico")]
    [ProducesResponseType(typeof(PaginacaoDto<TarefaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultadoOperacao), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListarTarefas(int projetoId, [FromHeader] int numeroPagina)
    {
        try
        {
            var tarefasObtidas = await _tarefasService.ListarTarefas(projetoId, numeroPagina);

            if (tarefasObtidas.TotalItens == 0)
                return NotFound();

            return Ok(tarefasObtidas);
        }
        catch (RegraDeNegocioException ex)
        {
            var erroRegraNegocio = string.Format("{0} {1}", MensagensRegraNegocio.ErroRegraNegocio, ex.Message);

            _logger.LogWarning(ex, "{ErroRegraNegocio}", erroRegraNegocio);
            return BadRequest(new ResultadoOperacao(false, erroRegraNegocio));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{MensagemRegraNegocio} {MensagemErro}", MensagensRegraNegocio.ErroExcecaoDesconhecido, ex.Message);
            return BadRequest(new ResultadoOperacao(false, MensagensRegraNegocio.Erro400));
        }
    }

    [HttpPost]
    [EndpointDescription("Cria uma nova tarefa em um projeto")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResultadoOperacao), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarTarefa([FromBody] TarefaDto tarefa)
    {
        try
        {
            await _tarefasService.Criar(tarefa);
            return Created();
        }
        catch (RegraDeNegocioException ex)
        {
            var erroRegraNegocio = string.Format("{0} {1}", MensagensRegraNegocio.ErroRegraNegocio, ex.Message);

            _logger.LogWarning(ex, "{ErroRegraNegocio}", erroRegraNegocio);
            return BadRequest(new ResultadoOperacao(false, erroRegraNegocio));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{MensagemRegraNegocio} {MensagemErro}", MensagensRegraNegocio.ErroExcecaoDesconhecido, ex.Message);
            return BadRequest(new ResultadoOperacao(false, $"{MensagensRegraNegocio.Erro400}"));
        }
    }

    [HttpPut()]
    [EndpointDescription("Atualiza o status ou detalhes de uma tarefa")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResultadoOperacao), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AtualizarTarefa([FromBody] TarefaDto tarefa)
    {
        try
        {
            await _tarefasService.Atualizar(tarefa);
            return NoContent();
        }
        catch (RegraDeNegocioException ex)
        {
            var erroRegraNegocio = string.Format("{0} {1}", MensagensRegraNegocio.ErroRegraNegocio, ex.Message);

            _logger.LogWarning(ex, "{ErroRegraNegocio}", erroRegraNegocio);
            return BadRequest(new ResultadoOperacao(false, erroRegraNegocio));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{MensagemRegraNegocio} {MensagemErro}", MensagensRegraNegocio.ErroExcecaoDesconhecido, ex.Message);
            return BadRequest(new ResultadoOperacao(false, $"{MensagensRegraNegocio.Erro400}"));
        }
    }

    [HttpDelete("{tarefaId:int}")]
    [EndpointDescription("Remove uma tarefa de um projeto")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResultadoOperacao), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoverTarefa(int tarefaId)
    {
        try
        {
            await _tarefasService.Remover(tarefaId);
            return NoContent();
        }
        catch (RegraDeNegocioException ex)
        {
            var erroRegraNegocio = string.Format("{0} {1}", MensagensRegraNegocio.ErroRegraNegocio, ex.Message);

            _logger.LogWarning(ex, "{ErroRegraNegocio}", erroRegraNegocio);
            return BadRequest(new ResultadoOperacao(false, erroRegraNegocio));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{MensagemRegraNegocio} {MensagemErro}", MensagensRegraNegocio.ErroExcecaoDesconhecido, ex.Message);
            return BadRequest(new ResultadoOperacao(false, $"{MensagensRegraNegocio.Erro400}"));
        }
    }
}
