using GerenciadorTarefas.Dominio.Entidades;

namespace GerenciadorTarefas.Dominio.Interfaces;

public interface ITarefasRepository
{
    Task<List<Tarefa>> ListarTarefasAsync(int projetoId, int numeroPagina, int itensPorPagina);
    Task<Tarefa?> ObterAsync(int tarefaId);
    Task CriarAsync(Tarefa tarefa);
    Task AtualizarAsync(Tarefa tarefa);
    Task RemoverAsync(Tarefa tarefa);
    Task<int> TotalDeTarefasPorProjetoAsync(int projetoId);
    Task<int> TotalDeTarefasNaoConcluidasPorProjetoAsync(int projetoId);
}
