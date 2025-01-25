using GerenciadorTarefas.Aplicacao.Interfaces;
using GerenciadorTarefas.Dominio.Entidades;
using GerenciadorTarefas.Dominio.Enumeradores;
using GerenciadorTarefas.Dominio.Interfaces;
using Microsoft.Extensions.Logging;

namespace GerenciadorTarefas.Aplicacao.Servicos
{
    public class HistoricosTarefasService : IHistoricosTarefasService
    {
        private readonly ILogger<HistoricosTarefasService> _logger;
        private readonly IHistoricosTarefasRepository _historicosTarefasRepository;

        public HistoricosTarefasService(ILogger<HistoricosTarefasService> logger, IHistoricosTarefasRepository historicosTarefasRepository)
        {
            _logger = logger;
            _historicosTarefasRepository = historicosTarefasRepository;
        }

        public async Task CriarHistorico(Tarefa tarefa, OperacaoRealizada operacaoRealizada)
        {
            var historico = new HistoricoTarefa(operacaoRealizada, tarefa, DateTime.Now);
            await _historicosTarefasRepository.CriarHistoricoAsync(historico);
        }
    }
}
