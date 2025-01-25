using GerenciadorTarefas.Dominio.Enumeradores;
using GerenciadorTarefas.Dominio.Validacoes;

namespace GerenciadorTarefas.Dominio.Entidades;

public class HistoricoTarefa : Entity
{
    public DateTime DataCadastro { get; private set; }
    
    public OperacaoRealizada OperacaoRealizada { get; private set; }

    public int TarefaId { get; private set; }

    public string Titulo { get; private set; }

    public string Descricao { get; private set; }

    public DateTime DataVencimento { get; private set; }

    public StatusTarefa Status { get; private set; }

    public PrioridadeTarefa Prioridade { get; private set; }

    public int ProjetoId { get; private set; }

    public int UsuarioId { get; private set; }

    public HistoricoTarefa(OperacaoRealizada operacaoRealizada, Tarefa tarefa, DateTime dataCadastro)
    {
        OperacaoRealizada = operacaoRealizada;
        TarefaId = tarefa.Id;
        Titulo = tarefa.Titulo;
        Descricao = tarefa.Descricao;
        DataVencimento = tarefa.DataVencimento;
        Status = tarefa.Status;
        Prioridade = tarefa.Prioridade;
        ProjetoId = tarefa.ProjetoId;
        UsuarioId = tarefa.UsuarioId;
        
        ValidarDominio();
    }

    public HistoricoTarefa(int id, OperacaoRealizada operacaoRealizada, Tarefa tarefa, DateTime dataCadastro)
        : this(operacaoRealizada, tarefa, dataCadastro)
    {
        Id = id;
        ValidacaoDominioException.Validar(Id < 0, "Id do histórico da tarefa inválido!");   
    }

    protected override void ValidarDominio()
    {
        ValidacaoDominioException.Validar(TarefaId < 0, "Id da tarefa inválido!");
        ValidacaoDominioException.Validar(ProjetoId < 0, "Id do projeto inválido!");
        ValidacaoDominioException.Validar(UsuarioId < 0, "Id do usuário inválido!");
    }
}
