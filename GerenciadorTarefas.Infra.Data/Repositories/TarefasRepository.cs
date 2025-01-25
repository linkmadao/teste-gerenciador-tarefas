using GerenciadorTarefas.Dominio.Entidades;
using GerenciadorTarefas.Dominio.Enumeradores;
using GerenciadorTarefas.Dominio.Interfaces;
using GerenciadorTarefas.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorTarefas.Infra.Data.Repositories;

public class TarefasRepository(ApplicationContext context) : ITarefasRepository
{
    private readonly ApplicationContext _context = context;

    public async Task<List<Tarefa>> ListarTarefasAsync(int projetoId, int numeroPagina, int itensPorPagina)
    {
        return await _context.Tarefas
            .Where(t => t.ProjetoId == projetoId && !t.Deletado)
            .Skip((numeroPagina - 1) * itensPorPagina)
            .Take(10)
            .ToListAsync();
    }

    public async Task<Tarefa?> ObterAsync(int tarefaId)
    {
        return await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == tarefaId && !t.Deletado);
    }

    public async Task CriarAsync(Tarefa tarefa)
    {
        _context.Tarefas.Add(tarefa);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Tarefa tarefa)
    {
        _context.Tarefas.Update(tarefa);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Tarefa tarefa)
    {
        _context.Tarefas.Update(tarefa);
        await _context.SaveChangesAsync();
    }

    public async Task<int> TotalDeTarefasPorProjetoAsync(int projetoId)
    {
        return await _context.Tarefas.CountAsync(t => t.ProjetoId == projetoId);
    }

    public async Task<int> TotalDeTarefasNaoConcluidasPorProjetoAsync(int projetoId)
    {
        return await _context.Tarefas
            .Where(t => t.Status != StatusTarefa.Concluida && !t.Deletado)
            .CountAsync(t => t.ProjetoId == projetoId);
    }
}
