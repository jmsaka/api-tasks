namespace TaskManagement.Api.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapperServices();
        services.AddValidationServices();
        services.AddMediatRServices();
        services.AddLoggingServices(configuration);
        services.AddRepositoryServices();
        services.AddCustomServices();

        return services;
    }

    private static IServiceCollection AddAutoMapperServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(TarefaProfile).Assembly);
        return services;
    }

    private static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(UpsertTarefaCommandValidator).Assembly);
        return services;
    }

    private static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(UpsertTarefaCommandHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(UpsertProjetoCommandHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(DeleteProjetoCommandHandler).Assembly);
        });

        services.AddScoped<IRequestHandler<UpsertTarefaCommand, BaseResponse<Guid>>, UpsertTarefaCommandHandler>();
        return services;
    }

    private static IServiceCollection AddLoggingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            loggingBuilder.AddNLog(configuration);
        });
        return services;
    }

    private static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IHistoricoAtualizacaoService, HistoricoAtualizacaoService>();
        return services;
    }
}
