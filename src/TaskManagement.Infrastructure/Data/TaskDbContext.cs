namespace TaskManagement.Infrastructure.Data;

public class TaskDbContext(DbContextOptions<TaskDbContext> options) : DbContext(options)
{
    public required DbSet<ProjetoEntity> Projetos { get; set; }
    public required DbSet<TarefaEntity> Tarefas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjetoEntity>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<TarefaEntity>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<ProjetoEntity>()
            .HasMany(p => p.Tarefas)
            .WithOne(t => t.Projeto)
            .HasForeignKey(t => t.ProjetoId);
    }
}
