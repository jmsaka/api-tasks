namespace TaskManagement.Api.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(TarefaProfile).Assembly);

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(UpsertTarefaCommandValidator).Assembly);


        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(UpsertTarefaCommandHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(UpsertProjetoCommandHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(DeleteProjetoCommandHandler).Assembly);
        });


        // Registro de Handlers e Validators
        services.AddScoped<IRequestHandler<UpsertTarefaCommand, BaseResponse<Guid>>, UpsertTarefaCommandHandler>();

        // NLog
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            loggingBuilder.AddNLog(configuration);
        });

        // Repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}