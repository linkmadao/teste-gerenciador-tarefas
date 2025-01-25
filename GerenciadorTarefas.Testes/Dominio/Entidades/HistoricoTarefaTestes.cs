using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Testes.Dominio.Entidades
{
    public class HistoricoTarefaTestes
    {
        [Fact]
        public void HistoricoTarefa_Ok()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            var descricao = "Descrição da tarefa";
            var dataCriacao = DateTime.Now;

            // Act
            var historicoTarefa = new HistoricoTarefa(tarefaId, descricao, dataCriacao);

            // Assert
            Assert.Equal(tarefaId, historicoTarefa.TarefaId);
            Assert.Equal(descricao, historicoTarefa.Descricao);
            Assert.Equal(dataCriacao, historicoTarefa.DataCriacao);
        }

        [Fact]
        public void DeveAtualizarDescricaoHistoricoTarefa()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            var descricao = "Descrição da tarefa";
            var dataCriacao = DateTime.Now;
            var historicoTarefa = new HistoricoTarefa(tarefaId, descricao, dataCriacao);
            var novaDescricao = "Nova descrição da tarefa";

            // Act
            historicoTarefa.AtualizarDescricao(novaDescricao);

            // Assert
            Assert.Equal(novaDescricao, historicoTarefa.Descricao);
        }

        [Fact]
        public void DeveAtualizarDataCriacaoHistoricoTarefa()
        {
            // Arrange
            var tarefaId = Guid.NewGuid();
            var descricao = "Descrição da tarefa";
            var dataCriacao = DateTime.Now;
            var historicoTarefa = new HistoricoTarefa(tarefaId, descricao, dataCriacao);
            var novaDataCriacao = DateTime.Now.AddDays(1);

            // Act
            historicoTarefa.AtualizarDataCriacao(novaDataCriacao);

            // Assert
            Assert.Equal(novaDataCriacao, historicoTarefa.DataCriacao);
        }
    }
}
