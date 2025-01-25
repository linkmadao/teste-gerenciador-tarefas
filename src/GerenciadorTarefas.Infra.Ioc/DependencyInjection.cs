using GerenciadorTarefas.Aplicacao.Interfaces;
using GerenciadorTarefas.Aplicacao.Servicos;
using GerenciadorTarefas.Dominio.Interfaces;
using GerenciadorTarefas.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace GerenciadorTarefas.Infra.Ioc;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection InstanciarInjecaoDependenciaRepositorios(this IServiceCollection services)
    {
        services.AddTransient<ITarefasRepository, TarefasRepository>();

        return services;
    }

    public static IServiceCollection InstanciarInjecaoDependenciaServicos(this IServiceCollection services)
    {
        services.AddTransient<ITarefasService, TarefasService>();

        return services;
    }
}