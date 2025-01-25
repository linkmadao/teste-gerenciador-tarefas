using GerenciadorTarefas.Dominio.Entidades;

namespace GerenciadorTarefas.Dominio.Interfaces;

public interface IHistoricosTarefasRepository
{
    Task CriarHistoricoAsync(HistoricoTarefa historicoTarefa);
}
