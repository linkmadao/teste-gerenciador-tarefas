using GerenciadorTarefas.Aplicacao.Dtos;

namespace GerenciadorTarefas.Aplicacao.Interfaces;

public interface ITarefasService
{
    Task<PaginacaoDto<TarefaDto>> ListarTarefas(int projetoId, int numeroPagina);

    Task Criar(TarefaDto tarefa);

    Task Atualizar(TarefaDto tarefa);

    Task Remover(int tarefaId);
}
