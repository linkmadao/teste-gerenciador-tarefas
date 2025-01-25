using GerenciadorTarefas.Dominio.Enumeradores;

namespace GerenciadorTarefas.Aplicacao.Dtos;

public class TarefaDto
{
    public int Id { get; set; }

    public bool Deletado { get; set; }

    public required string Titulo { get; set; }

    public string Descricao { get; set; } = string.Empty;

    public DateTime DataCriacao { get; set; }

    public DateTime DataVencimento { get; set; }

    public StatusTarefa Status { get; set; }

    public required PrioridadeTarefa Prioridade { get; set; }

    public List<ComentarioDto> Comentarios { get; set; } = [];

    public required int ProjetoId { get; set; }

    public int UsuarioId { get; set; }
}
