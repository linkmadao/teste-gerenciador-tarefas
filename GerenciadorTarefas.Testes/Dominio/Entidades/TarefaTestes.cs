using GerenciadorTarefas.Dominio.Entidades;
using GerenciadorTarefas.Dominio.Enumeradores;
using GerenciadorTarefas.Dominio.Validacoes;
using GerenciadorTarefas.Testes.Traits;

namespace GerenciadorTarefas.Testes.Dominio.Entidades
{
    public class TarefaTestes
    {
        [Fact]
        public void Tarefa_Ok()
        {
            // Arrange & Act
            var tarefa = TarefasTraits.Tarefa();
            tarefa.Projeto = new Projeto(1, "Projeto 1");

            // Assert
            Assert.Equal(1, tarefa.Id);
            Assert.Equal("Tarefa 1", tarefa.Titulo);
            Assert.Equal("Descricao 1", tarefa.Descricao);
            Assert.Equal(DateTime.Now.AddDays(-5).Date, tarefa.DataCriacao.Date);
            Assert.Equal(DateTime.Now.AddDays(5).Date, tarefa.DataVencimento.Date);
            Assert.Equal(StatusTarefa.EmAndamento, tarefa.Status);
            Assert.Equal(PrioridadeTarefa.Media, tarefa.Prioridade);
            Assert.Empty(tarefa.Comentarios);
            Assert.Equal(1, tarefa.ProjetoId);
            Assert.NotNull(tarefa.Projeto);
            Assert.Equal(1, tarefa.UsuarioId);
        }

        [Fact]
        public void Tarefa_RegraDeNegocioException_QuandoValidarId()
        {
            // Arrange & Act
            var tarefa = TarefasTraits.TarefaComIdInvalido;

            // Act
            Assert.Throws<ValidacaoDominioException>(tarefa);
        }

        [Fact]
        public void Tarefa_RegraDeNegocioException_QuandoValidarTitulo()
        {
            // Arrange & Act
            var tarefa = TarefasTraits.TarefaComTituloInvalido;

            // Act
            Assert.Throws<ValidacaoDominioException>(tarefa);
        }

        [Fact]
        public void Tarefa_RegraDeNegocioException_QuandoValidarDataCriacaoMinima()
        {
            // Arrange & Act
            var tarefa = TarefasTraits.TarefaComDataCriacaoMinima;

            // Act
            Assert.Throws<ValidacaoDominioException>(tarefa);
        }

        [Fact]
        public void Tarefa_RegraDeNegocioException_QuandoValidarDataCriacaoMaxima()
        {
            // Arrange & Act
            var tarefa = TarefasTraits.TarefaComDataCriacaoMaxima;

            // Act
            Assert.Throws<ValidacaoDominioException>(tarefa);
        }

        [Fact]
        public void Tarefa_RegraDeNegocioException_QuandoValidarDataVencimentoMinima()
        {
            // Arrange & Act
            var tarefa = TarefasTraits.TarefaComDataVencimentoMinima;

            // Act
            Assert.Throws<ValidacaoDominioException>(tarefa);
        }

        [Fact]
        public void Tarefa_RegraDeNegocioException_QuandoValidarDataVencimentoMaxima()
        {
            // Arrange & Act
            var tarefa = TarefasTraits.TarefaComDataVencimentoMaxima;

            // Act
            Assert.Throws<ValidacaoDominioException>(tarefa);
        }

        [Fact]
        public void Tarefa_RegraDeNegocioException_QuandoValidarProjetoId()
        {
            // Arrange & Act
            var tarefa = TarefasTraits.TarefaComProjetoIdInvalido;

            // Act
            Assert.Throws<ValidacaoDominioException>(tarefa);
        }

        [Fact]
        public void Tarefa_RegraDeNegocioException_QuandoValidarUsuarioId()
        {
            // Arrange & Act
            var tarefa = TarefasTraits.TarefaComUsuarioIdInvalido;

            // Act
            Assert.Throws<ValidacaoDominioException>(tarefa);
        }

        [Fact]
        public void Tarefa_Ok_DeveAdicionarComentarios()
        {
            // Arrange
            var tarefa = TarefasTraits.Tarefa();

            // Act
            tarefa.AdicionarComentarios(
            [
                new Comentario(1, 1, 1, "Comentario 1")
            ]);

            // Assert
            Assert.NotEmpty(tarefa.Comentarios);
        }

        [Fact]
        public void Tarefa_Ok_DeveAtualizarStatus()
        {
            // Arrange
            var tarefa = TarefasTraits.Tarefa();
            var statusOriginal = tarefa.Status;

            // Act
            tarefa.AtualizarStatus(StatusTarefa.Concluida);

            // Assert
            Assert.NotEqual(statusOriginal, tarefa.Status);
        }

        [Fact]
        public void Tarefa_Ok_DeveAtualizarDescricao()
        {
            // Arrange
            var tarefa = TarefasTraits.Tarefa();
            var descricaoOriginal = tarefa.Descricao;

            // Act
            tarefa.AtualizarDescricao("Nova Descricao");

            // Assert
            Assert.NotEqual(descricaoOriginal, tarefa.Descricao);
        }
    }
}
