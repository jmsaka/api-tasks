namespace TaskManagement.Infrastructure.Data;

public class TaskDbContext : DbContext
{
    public required DbSet<ProjetoEntity> Projetos { get; set; }
    public required DbSet<TarefaEntity> Tarefas { get; set; }

    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

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
