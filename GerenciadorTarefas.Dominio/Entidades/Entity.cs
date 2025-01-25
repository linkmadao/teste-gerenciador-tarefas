namespace GerenciadorTarefas.Dominio.Entidades;

public abstract class Entity
{
    public int Id { get; protected set; }

    public bool Deletado { get; protected set; }

    public DateTime DataCriacao { get; protected set; }

    protected abstract void ValidarDominio();

    public void SetarComoDeletado()
    {
        Deletado = true;
    }
}
