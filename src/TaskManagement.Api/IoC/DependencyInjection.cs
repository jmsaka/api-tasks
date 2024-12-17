using TaskManagement.Application.Commands.Tarefas;
using TaskManagement.Application.Profiles;
using TaskManagement.Application.Validators;

namespace TaskManagement.Api.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuração do DbContext
        services.AddDbContext<TaskDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // AutoMapper
        services.AddAutoMapper(typeof(TarefaProfile).Assembly);

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(CreateTarefaCommandValidator).Assembly);


        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTarefaCommandHandler).Assembly));


        // Registro de Handlers e Validators
        services.AddScoped<IRequestHandler<CreateTarefaCommand, Guid>, CreateTarefaCommandHandler>();

        // NLog
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            loggingBuilder.AddNLog(configuration);
        });

        return services;
    }
}