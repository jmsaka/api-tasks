namespace TaskManagement.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<TaskDbContext>(options =>
        {
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
            );
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        // Add services to the container.
        DependencyInjection.AddApplicationServices(builder.Services, builder.Configuration);

        var environmentValue = builder.Configuration["Environment"];

        if (string.Equals(environmentValue, "Production", StringComparison.Ordinal))
        {
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 51000);
            });
        }

        //builder.WebHost.ConfigureKestrel(options =>
        //{
        //    options.Listen(IPAddress.Any, 51000);
        //});

        var app = builder.Build();

        // Adiciona o middleware de fake authentication
        app.UseMiddleware<FakeAuthenticationMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();

                if (dbContext.Database.CanConnect())
                {
                    Console.WriteLine("Conectado");
                    dbContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        app.Run();
    }
}
