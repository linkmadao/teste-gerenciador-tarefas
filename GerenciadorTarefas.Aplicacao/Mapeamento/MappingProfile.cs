using AutoMapper;
using GerenciadorTarefas.Aplicacao.Dtos;
using GerenciadorTarefas.Dominio.Entidades;
using System.Diagnostics.CodeAnalysis;

namespace GerenciadorTarefas.Aplicacao.Mapeamento;

[ExcludeFromCodeCoverage]
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TarefaDto, Tarefa>().ReverseMap();

        CreateMap<List<TarefaDto>, List<Tarefa>>().ReverseMap();

        CreateMap<ComentarioDto, Comentario>().ReverseMap();

        //CreateMap<ProjetoDto, Projeto>()
        //  .ReverseMap();
    }
}
