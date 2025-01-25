namespace GerenciadorTarefas.Aplicacao.Dtos;

public class PaginacaoDto<T>
{
    public int Pagina { get; set; }

    private readonly int TamanhoPagina = 10;

    public int TotalPaginas => (int)Math.Ceiling((double)TotalItens / TamanhoPagina);

    public int TotalItens => Dados.Count();

    public IEnumerable<T> Dados { get; set; } = [];
}
