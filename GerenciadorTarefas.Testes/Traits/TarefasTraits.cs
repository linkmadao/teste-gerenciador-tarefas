using GerenciadorTarefas.Aplicacao.Dtos;
using GerenciadorTarefas.Dominio.Entidades;
using GerenciadorTarefas.Dominio.Enumeradores;

namespace GerenciadorTarefas.Testes.Traits
{
    public static class TarefasTraits
    {
        public static TarefaDto TarefaDto()
        {
            return new()
            {
                Id = 1,
                Titulo = "Tarefa 1",
                Status = StatusTarefa.EmAndamento,
                Prioridade = PrioridadeTarefa.Media,
                Descricao = "Descricao 1",
                DataCriacao = DateTime.Now.AddDays(-5),
                DataVencimento = DateTime.Now.AddDays(5),
                ProjetoId = 1,
                UsuarioId = 1
            };
        }

        public static List<TarefaDto> ListaTarefasDto()
        {
            var tarefas = new List<TarefaDto>
            {
                TarefaDto()
            };

            return tarefas;
        }

        public static PaginacaoDto<TarefaDto> PaginacaoTarefas()
        {
            return new()
            {
                Pagina = 1,
                Dados = ListaTarefasDto()
            };
        }

        public static Tarefa Tarefa()
        {
            return new
            (
                id: 1,
                titulo: "Tarefa 1",
                descricao: "Descricao 1",
                dataCriacao: DateTime.Now.AddDays(-5),
                dataVencimento: DateTime.Now.AddDays(5),
                status: StatusTarefa.EmAndamento,
                prioridade: PrioridadeTarefa.Media,
                comentarios: [],
                projetoId: 1,
                usuarioId: 1
            );
        }

        public static Tarefa TarefaComIdInvalido()
        {
            var tarefa = Tarefa();

            return new
            (
                id: -1,
                titulo: tarefa.Titulo,
                descricao: tarefa.Descricao,
                dataCriacao: tarefa.DataCriacao,
                dataVencimento: tarefa.DataVencimento,
                status: tarefa.Status,
                prioridade: tarefa.Prioridade,
                comentarios: tarefa.Comentarios,
                projetoId: tarefa.ProjetoId,
                usuarioId: tarefa.UsuarioId
            );
        }

        public static Tarefa TarefaComTituloInvalido()
        {
            var tarefa = Tarefa();

            return new
            (
                id: tarefa.Id,
                titulo: "",
                descricao: tarefa.Descricao,
                dataCriacao: tarefa.DataCriacao,
                dataVencimento: tarefa.DataVencimento,
                status: tarefa.Status,
                prioridade: tarefa.Prioridade,
                comentarios: tarefa.Comentarios,
                projetoId: tarefa.ProjetoId,
                usuarioId: tarefa.UsuarioId
            );
        }

        public static Tarefa TarefaComDataCriacaoMinima()
        {
            var tarefa = Tarefa();

            return new
            (
                id: tarefa.Id,
                titulo: tarefa.Titulo,
                descricao: tarefa.Descricao,
                dataCriacao: DateTime.MinValue,
                dataVencimento: tarefa.DataVencimento,
                status: tarefa.Status,
                prioridade: tarefa.Prioridade,
                comentarios: tarefa.Comentarios,
                projetoId: tarefa.ProjetoId,
                usuarioId: tarefa.UsuarioId
            );
        }

        public static Tarefa TarefaComDataCriacaoMaxima()
        {
            var tarefa = Tarefa();

            return new
            (
                id: tarefa.Id,
                titulo: tarefa.Titulo,
                descricao: tarefa.Descricao,
                dataCriacao: DateTime.MaxValue,
                dataVencimento: tarefa.DataVencimento,
                status: tarefa.Status,
                prioridade: tarefa.Prioridade,
                comentarios: tarefa.Comentarios,
                projetoId: tarefa.ProjetoId,
                usuarioId: tarefa.UsuarioId
            );
        }

        public static Tarefa TarefaComDataVencimentoMinima()
        {
            var tarefa = Tarefa();

            return new
            (
                id: tarefa.Id,
                titulo: tarefa.Titulo,
                descricao: tarefa.Descricao,
                dataCriacao: tarefa.DataCriacao,
                dataVencimento: DateTime.MinValue,
                status: tarefa.Status,
                prioridade: tarefa.Prioridade,
                comentarios: tarefa.Comentarios,
                projetoId: tarefa.ProjetoId,
                usuarioId: tarefa.UsuarioId
            );
        }

        public static Tarefa TarefaComDataVencimentoMaxima()
        {
            var tarefa = Tarefa();

            return new
            (
                id: tarefa.Id,
                titulo: tarefa.Titulo,
                descricao: tarefa.Descricao,
                dataCriacao: tarefa.DataCriacao,
                dataVencimento: DateTime.MaxValue,
                status: tarefa.Status,
                prioridade: tarefa.Prioridade,
                comentarios: tarefa.Comentarios,
                projetoId: tarefa.ProjetoId,
                usuarioId: tarefa.UsuarioId
            );
        }

        public static Tarefa TarefaComProjetoIdInvalido()
        {
            var tarefa = Tarefa();

            return new
            (
                id: tarefa.Id,
                titulo: tarefa.Titulo,
                descricao: tarefa.Descricao,
                dataCriacao: tarefa.DataCriacao,
                dataVencimento: tarefa.DataVencimento,
                status: tarefa.Status,
                prioridade: tarefa.Prioridade,
                comentarios: tarefa.Comentarios,
                projetoId: -1,
                usuarioId: tarefa.UsuarioId
            );
        }

        public static Tarefa TarefaComUsuarioIdInvalido()
        {
            var tarefa = Tarefa();

            return new
            (
                id: tarefa.Id,
                titulo: tarefa.Titulo,
                descricao: tarefa.Descricao,
                dataCriacao: tarefa.DataCriacao,
                dataVencimento: tarefa.DataVencimento,
                status: tarefa.Status,
                prioridade: tarefa.Prioridade,
                comentarios: tarefa.Comentarios,
                projetoId: tarefa.ProjetoId,
                usuarioId: -1
            );
        }

        public static List<Tarefa> ListaTarefas()
        {
            var tarefas = new List<Tarefa>
            {
                Tarefa()
            };

            return tarefas;
        }
    }
}
