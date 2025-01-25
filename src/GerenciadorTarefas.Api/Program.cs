using GerenciadorTarefas.Infra.Ioc;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);

builder.Services.InstanciarInjecaoDependenciaRepositorios();
builder.Services.InstanciarInjecaoDependenciaServicos();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "GerenciadorTarefas.Api",
            Version = "1.0.0",
            Description = "Api de gestão de projetos e tarefas",
            Contact = new OpenApiContact
            {
                Name = "eclipseworks",
                Email = "contato@eclipseworks.com.br"
            }
        }
    );
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "GerenciadorTarefas.Api");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();