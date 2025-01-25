using GerenciadorTarefas.Dominio.Constantes;

namespace GerenciadorTarefas.Testes.Traits
{
    public static class ConstantesTraits
    {
        public static string MensagemErroRegraNegocio(string mensagemErro)
        {
            var erroRegraNegocio = string.Format("{0} {1}", MensagensRegraNegocio.ErroRegraNegocio, mensagemErro);

            return erroRegraNegocio;
        }
    }
}
