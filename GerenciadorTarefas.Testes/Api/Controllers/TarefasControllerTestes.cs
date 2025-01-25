using GerenciadorTarefas.Api.Controllers;
using GerenciadorTarefas.Api.Responses;
using GerenciadorTarefas.Aplicacao.Dtos;
using GerenciadorTarefas.Aplicacao.Interfaces;
using GerenciadorTarefas.Dominio.Excecoes;
using GerenciadorTarefas.Testes.Traits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace GerenciadorTarefas.Testes.Api.Controllers;

public class TarefasControllerTestes
{
    private readonly Mock<ILogger<TarefasController>> _mockLogger;
    private readonly Mock<ITarefasService> _mockTarefasService;
    private readonly TarefasController _controller;

    public TarefasControllerTestes()
    {
        _mockLogger = new Mock<ILogger<TarefasController>>();
        _mockTarefasService = new Mock<ITarefasService>();
        _controller = new TarefasController(_mockLogger.Object, _mockTarefasService.Object);
    }

    #region ListarTarefas
    [Fact]
    public async Task ListarTarefas_Ok_QuandoEncontrarItens_ValidaTipoRespostaRetornada()
    {
        // Arrange
        var paginacaoTarefas = TarefasTraits.PaginacaoTarefas();

        _mockTarefasService
            .Setup(service => service.ListarTarefas(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(paginacaoTarefas);

        // Act
        var tarafasObtidas = await _controller.ListarTarefas(1, 1);

        // Assert
        Assert.IsType<OkObjectResult>(tarafasObtidas);
    }

    [Fact]
    public async Task ListarTarefas_Ok_QuandoEncontrarItens_ValidaTipoListaRetornada()
    {
        // Arrange
        var listaTarefas = TarefasTraits.PaginacaoTarefas();

        _mockTarefasService
            .Setup(service => service.ListarTarefas(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(listaTarefas);

        // Act
        var tarefasObtidas = await _controller.ListarTarefas(1, 1);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(tarefasObtidas);
        Assert.IsAssignableFrom<PaginacaoDto<TarefaDto>>(okObjectResult.Value);
    }

    [Fact]
    public async Task ListarTarefas_Ok_QuandoEncontrarItens_ValidaItemRetornado()
    {
        // Arrange
        var listaTarefas = TarefasTraits.PaginacaoTarefas();

        _mockTarefasService
            .Setup(service => service.ListarTarefas(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(listaTarefas);

        // Act
        var tarefasObtidas = await _controller.ListarTarefas(1, 1);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(tarefasObtidas);
        var valorRetornado = Assert.IsAssignableFrom<PaginacaoDto<TarefaDto>>(okObjectResult.Value);
        Assert.Equal(listaTarefas.TotalItens, valorRetornado.Dados.Count());
    }

    [Fact]
    public async Task ListarTarefas_NotFound_QuandoNaoEncontrarItens()
    {
        // Arrange
        _mockTarefasService
            .Setup(service => service.ListarTarefas(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new PaginacaoDto<TarefaDto>());

        // Act
        var tarefasObtidas = await _controller.ListarTarefas(1, 1);

        // Assert
        Assert.IsType<NotFoundResult>(tarefasObtidas);
    }

    [Fact]
    public async Task ListarTarefas_BadRequest_QuandoOcorrerErroNaRegraDeNegocio_ValidaTipoRespostaRetornada()
    {
        // Arrange
        _mockTarefasService
            .Setup(service => service.ListarTarefas(It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new RegraDeNegocioException("Teste retorno regra negócio"));

        // Act
        var respostaBadRequest = await _controller.ListarTarefas(1, 1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(respostaBadRequest);
    }

    [Fact]
    public async Task ListarTarefas_BadRequest_QuandoOcorrerErroNaRegraDeNegocio_ValidaStatusOperacaoRetornada()
    {
        // Arrange
        var mensagemErro = "Teste erro regra negócio";
        var mensagemErroRegraNegocio = ConstantesTraits.MensagemErroRegraNegocio(mensagemErro);

        _mockTarefasService
            .Setup(service => service.ListarTarefas(It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new RegraDeNegocioException(mensagemErro));

        // Act
        var respostaBadRequest = await _controller.ListarTarefas(1, 1);

        // Assert
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(respostaBadRequest);
        var resultadoOperacao = Assert.IsAssignableFrom<ResultadoOperacao>(badRequestObjectResult.Value);
        Assert.False(resultadoOperacao.Sucesso);
    }

    [Fact]
    public async Task ListarTarefas_BadRequest_QuandoOcorrerErroNaRegraDeNegocio_ValidaMensagemRetornada()
    {
        // Arrange
        var mensagemErro = "Teste erro regra negócio";
        var mensagemErroRegraNegocio = ConstantesTraits.MensagemErroRegraNegocio(mensagemErro);

        _mockTarefasService
            .Setup(service => service.ListarTarefas(It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new RegraDeNegocioException(mensagemErro));

        // Act
        var respostaBadRequest = await _controller.ListarTarefas(1, 1);

        // Assert
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(respostaBadRequest);
        var resultadoOperacao = Assert.IsAssignableFrom<ResultadoOperacao>(badRequestObjectResult.Value);
        Assert.Equal(mensagemErroRegraNegocio, resultadoOperacao.Mensagens.First());
    }

    [Fact]
    public async Task ListarTarefas_BadRequest_QuandoOcorrerErroNaRegraDeNegocio_ValidaLogWarning()
    {
        // Arrange
        var mensagemErro = "Teste retorno regra negócio";
        var mensagemErroRegraNegocio = ConstantesTraits.MensagemErroRegraNegocio(mensagemErro);

        _mockTarefasService
            .Setup(service => service.ListarTarefas(It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new RegraDeNegocioException(mensagemErroRegraNegocio));

        // Act
        var respostaBadRequest = await _controller.ListarTarefas(1, 1);

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
    public async Task ListarTarefas_BadRequest_QuandoOcorrerErroNaoMapeado()
    {
        // Arrange
        _mockTarefasService
            .Setup(service => service.ListarTarefas(It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new ArgumentNullException());

        // Act
        var respostaBadRequest = await _controller.ListarTarefas(1, 1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(respostaBadRequest);
    }

    [Fact]
    public async Task ListarTarefas_BadRequest_QuandoOcorrerErroNaoMapeado_ValidaLogError()
    {
        // Arrange
        var mensagemErro = "Parâmetro inválido";

        _mockTarefasService
            .Setup(service => service.ListarTarefas(It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new ArgumentNullException(mensagemErro));

        // Act
        var respostaBadRequest = await _controller.ListarTarefas(1, 1);

        // Assert
        _mockLogger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }
    #endregion

    #region CriarTarefa
    [Fact]
    public async Task CriarTarefa_Created_QuandoTarefaForCriada()
    {
        // Arrange
        var tarefa = TarefasTraits.TarefaDto();

        _mockTarefasService.Setup(service => service.Criar(It.IsAny<TarefaDto>()));

        // Act
        var resultado = await _controller.CriarTarefa(tarefa);

        // Assert
        Assert.IsType<CreatedResult>(resultado);
    }

    [Fact]
    public async Task CriarTarefa_BadRequest_QuandoOcorrerErroNaRegraDeNegocio()
    {
        // Arrange
        var tarefa = TarefasTraits.TarefaDto();

        _mockTarefasService
            .Setup(service => service.Criar(It.IsAny<TarefaDto>()))
            .ThrowsAsync(new RegraDeNegocioException("Teste retorno regra negócio"));

        // Act
        var respostaBadRequest = await _controller.CriarTarefa(tarefa);

        // Assert
        Assert.IsType<BadRequestObjectResult>(respostaBadRequest);
    }

    [Fact]
    public async Task CriarTarefa_BadRequest_QuandoOcorrerErroNaoMapeado()
    {
        // Arrange
        var tarefa = TarefasTraits.TarefaDto();

        _mockTarefasService
            .Setup(service => service.Criar(It.IsAny<TarefaDto>()))
            .ThrowsAsync(new ArgumentNullException());

        // Act
        var respostaBadRequest = await _controller.CriarTarefa(tarefa);

        // Assert
        Assert.IsType<BadRequestObjectResult>(respostaBadRequest);
    }
    #endregion

    #region AtualizarTarefa
    [Fact]
    public async Task AtualizarTarefa_NoContent_QuandoTarefaForAtualizada()
    {
        // Arrange
        var tarefa = TarefasTraits.TarefaDto();

        _mockTarefasService.Setup(service => service.Atualizar(It.IsAny<TarefaDto>()));

        // Act
        var resultado = await _controller.AtualizarTarefa(tarefa);

        // Assert
        Assert.IsType<NoContentResult>(resultado);
    }

    [Fact]
    public async Task AtualizarTarefa_BadRequest_QuandoOcorrerErroNaRegraDeNegocio()
    {
        // Arrange
        var tarefa = TarefasTraits.TarefaDto();

        _mockTarefasService
             .Setup(service => service.Atualizar(It.IsAny<TarefaDto>()))
             .ThrowsAsync(new RegraDeNegocioException("Teste retorno regra negócio"));

        // Act
        var respostaBadRequest = await _controller.AtualizarTarefa(tarefa);

        // Assert
        Assert.IsType<BadRequestObjectResult>(respostaBadRequest);
    }

    [Fact]
    public async Task AtualizarTarefa_BadRequest_QuandoOcorrerErroNaoMapeado()
    {
        // Arrange
        var tarefa = TarefasTraits.TarefaDto();

        _mockTarefasService
            .Setup(service => service.Atualizar(It.IsAny<TarefaDto>()))
            .ThrowsAsync(new ArgumentNullException());

        // Act
        var respostaBadRequest = await _controller.AtualizarTarefa(tarefa);

        // Assert
        Assert.IsType<BadRequestObjectResult>(respostaBadRequest);
    }
    #endregion

    #region RemoverTarefa
    [Fact]
    public async Task RemoverTarefa_NoContent_QuandoTarefaForRemovida()
    {
        // Arrange
        var tarefaId = 1;
        var projetoId = 1;

        _mockTarefasService
            .Setup(service => service.Remover(tarefaId))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _controller.RemoverTarefa(tarefaId);

        // Assert
        Assert.IsType<NoContentResult>(resultado);
    }

    [Fact]
    public async Task RemoverTarefa_BadRequest_QuandoOcorrerErroNaRegraDeNegocio()
    {
        // Arrange
        var tarefaId = 1;
        var projetoId = 1;

        _mockTarefasService
             .Setup(service => service.Remover(tarefaId))
             .ThrowsAsync(new RegraDeNegocioException("Teste retorno regra negócio"));

        // Act
        var respostaBadRequest = await _controller.RemoverTarefa(tarefaId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(respostaBadRequest);
    }

    [Fact]
    public async Task RemoverTarefa_BadRequest_QuandoOcorrerErroNaoMapeado()
    {
        // Arrange
        var tarefaId = 1;
        var projetoId = 1;

        _mockTarefasService
            .Setup(service => service.Remover(tarefaId))
            .ThrowsAsync(new Exception("Erro ao remover tarefa"));

        // Act
        var resultado = await _controller.RemoverTarefa(tarefaId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(resultado);
    }
    #endregion
}
