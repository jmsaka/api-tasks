using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TaskManagement.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configurar o Kestrel para escutar na porta 51000
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 51000); 
        });

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        ////

        builder.Services.AddDbContext<TaskDbContext>(options =>
        {
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
            );
            options.EnableSensitiveDataLogging();
        });

        ////

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        // Executar migrações automaticamente na inicialização
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
            dbContext.Database.Migrate(); // Aplica as migrações
        }

        app.Run();
    }
}
