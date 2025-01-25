using GerenciadorTarefas.Dominio.Validacoes;

namespace GerenciadorTarefas.Dominio.Entidades;

public class Comentario : Entity
{
    public int TarefaId { get; private set; }

    public int UsuarioId { get; private set; }

    public string Texto { get; private set; }

    public Comentario(int tarefaId, int usuarioId, string texto)
    {
        TarefaId = tarefaId;
        UsuarioId = usuarioId;
        Texto = texto;

        ValidarDominio();
    }

    public Comentario(int id, int tarefaId, int usuarioId, string texto)
        : this(tarefaId, usuarioId, texto)
    {
        Id = id;
        ValidacaoDominioException.Validar(Id < 0, "Id do comentário inválido!");
    }

    protected override void ValidarDominio()
    {
        ValidacaoDominioException.Validar(string.IsNullOrWhiteSpace(Texto), "O texto do comentário não pode ser vazio!");
        ValidacaoDominioException.Validar(TarefaId < 0, "Id da tarefa inválido!");
        ValidacaoDominioException.Validar(UsuarioId < 0, "Id do usuario inválido!");
    }
}