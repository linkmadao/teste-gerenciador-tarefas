using GerenciadorTarefas.Dominio.Entidades;
using GerenciadorTarefas.Dominio.Enumeradores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace GerenciadorTarefas.Infra.Data.EntitiesConfigurations;

[ExcludeFromCodeCoverage]
public class TarefaConfiguration : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasMany(t => t.Comentarios)
            .WithOne()
            .HasForeignKey("TarefaId");

        GerarDados(builder);
    }

    private static void GerarDados(EntityTypeBuilder<Tarefa> builder)
    {
        builder.HasData(
            new Tarefa(1, "Tarefa 1", "Descricao 1", DateTime.Now, DateTime.Now.AddDays(5), StatusTarefa.Pendente, PrioridadeTarefa.Baixa, [], 1, 1),
            new Tarefa(2, "Tarefa 2", "Descricao 2", DateTime.Now, DateTime.Now.AddDays(5), StatusTarefa.EmAndamento, PrioridadeTarefa.Media, [], 1, 2),
            new Tarefa(3, "Tarefa 3", "Descricao 3", DateTime.Now, DateTime.Now.AddDays(5), StatusTarefa.Concluida, PrioridadeTarefa.Alta, [], 2, 1)
        );
    }
}
