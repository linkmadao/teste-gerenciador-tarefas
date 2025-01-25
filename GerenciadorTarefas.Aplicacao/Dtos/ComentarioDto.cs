namespace GerenciadorTarefas.Aplicacao.Dtos;

public class ComentarioDto
{
    public int Id { get; set; }

    public int TarefaId { get; set; }

    public int UsuarioId { get; set; }

    public string Texto { get; set; } = string.Empty;
}
