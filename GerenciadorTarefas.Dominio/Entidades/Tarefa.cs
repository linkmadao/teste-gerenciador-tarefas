using GerenciadorTarefas.Dominio.Enumeradores;
using GerenciadorTarefas.Dominio.Validacoes;

namespace GerenciadorTarefas.Dominio.Entidades;

public class Tarefa : Entity
{
    public string Titulo { get; private set; }

    public string Descricao { get; private set; }

    public DateTime DataVencimento { get; private set; }

    public StatusTarefa Status { get; private set; }

    public PrioridadeTarefa Prioridade { get; private set; }

    public List<Comentario> Comentarios { get; private set; } = [];

    public int ProjetoId { get; private set; }
    public Projeto Projeto { get; set; }

    public int UsuarioId { get; private set; }

    public Tarefa(string titulo, string descricao, DateTime dataCriacao, DateTime dataVencimento,
        StatusTarefa status, PrioridadeTarefa prioridade, List<Comentario> comentarios, int projetoId, int usuarioId)
    {
        Titulo = titulo;
        Descricao = descricao;
        DataCriacao = dataCriacao;
        DataVencimento = dataVencimento;
        Status = status;
        Prioridade = prioridade;
        Comentarios = comentarios;
        ProjetoId = projetoId;
        UsuarioId = usuarioId;

        ValidarDominio();
    }

    public Tarefa(int id, string titulo, string descricao, DateTime dataCriacao, DateTime dataVencimento,
        StatusTarefa status, PrioridadeTarefa prioridade, List<Comentario> comentarios, int projetoId, int usuarioId)
        : this(titulo, descricao, dataCriacao, dataVencimento, status, prioridade, comentarios, projetoId, usuarioId)
    {
        Id = id;
        ValidacaoDominioException.Validar(Id < 0, "Id da tarefa inválido!");
    }

    protected override void ValidarDominio()
    {
        ValidacaoDominioException.Validar(string.IsNullOrWhiteSpace(Titulo), "O titulo da tarefa não pode ser vazio!");
        ValidacaoDominioException.Validar(Titulo.Length < 3, "O titulo da tarefa deve conter ao menos 3 caracteres!");
        ValidacaoDominioException.Validar(DataCriacao == DateTime.MinValue || DataCriacao == DateTime.MaxValue, "Data de Criação inválida!");
        ValidacaoDominioException.Validar(DataVencimento == DateTime.MinValue || DataVencimento == DateTime.MaxValue, "Data de Vencimento inválida!");
        ValidacaoDominioException.Validar(ProjetoId < 0, "Id do projeto inválido!");
        ValidacaoDominioException.Validar(UsuarioId < 0, "Id do usuario inválido!");
    }

    public void AdicionarComentarios(List<Comentario> comentario) 
        => Comentarios.AddRange(comentario);

    public void AtualizarStatus(StatusTarefa status) 
        => Status = status;
    
    public void AtualizarDescricao(string descricao)
        => Descricao = descricao;
}
