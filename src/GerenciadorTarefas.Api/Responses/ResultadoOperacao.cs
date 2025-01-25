namespace GerenciadorTarefas.Api.Responses;

public class ResultadoOperacao
{
    public bool Sucesso { get; set; }
    public IEnumerable<string> Mensagens { get; set; }

    public ResultadoOperacao(bool sucesso, string mensagem)
    {
        Sucesso = sucesso;
        Mensagens = [mensagem];
    }
}
