namespace GerenciadorTarefas.Dominio.Validacoes;

public class ValidacaoDominioException(string erro) : Exception(erro)
{
    public static void Validar(bool existeErro, string erro)
    {
        if (existeErro)
            throw new ValidacaoDominioException(erro);
    }
}
