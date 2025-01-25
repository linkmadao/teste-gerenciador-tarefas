using GerenciadorTarefas.Dominio.Validacoes;

namespace GerenciadorTarefas.Dominio.Entidades;

public class Projeto : Entity
{
    public string Nome { get; private set; }
    public IEnumerable<Tarefa> ListaTarefas { get; set; }

    public Projeto(string nome)
    {
        Nome = nome;

        ValidarDominio();
    }

    public Projeto(int id, string nome) : this(nome)
    {
        Id = id;
        ValidacaoDominioException.Validar(Id < 0, "Id do Projeto inválido!");
    }

    protected override void ValidarDominio()
    {
        ValidacaoDominioException.Validar(string.IsNullOrWhiteSpace(Nome), "O nome do projeto não pode ser vazio!");
        ValidacaoDominioException.Validar(Nome.Length < 3, "O nome do projeto deve conter ao menos 3 caracteres!");
    }
}