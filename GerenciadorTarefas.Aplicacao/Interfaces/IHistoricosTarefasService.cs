using GerenciadorTarefas.Dominio.Entidades;
using GerenciadorTarefas.Dominio.Enumeradores;

namespace GerenciadorTarefas.Aplicacao.Interfaces;

public interface IHistoricosTarefasService
{
    Task CriarHistorico(Tarefa tarefa, OperacaoRealizada operacaoRealizada);
}
