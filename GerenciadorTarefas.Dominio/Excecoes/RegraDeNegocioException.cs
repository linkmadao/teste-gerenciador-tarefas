namespace GerenciadorTarefas.Dominio.Excecoes;

public class RegraDeNegocioException(string message) : Exception(message)
{
}