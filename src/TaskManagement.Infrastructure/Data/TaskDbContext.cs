namespace TaskManagement.Infrastructure.Data;

public class TaskDbContext : DbContext
{
    public required DbSet<ProjetoEntity> Projetos { get; set; }
    public required DbSet<TarefaEntity> Tarefas { get; set; }
    public required DbSet<ComentarioEntity> Comentarios { get; set; }

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

        modelBuilder.Entity<ComentarioEntity>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<ProjetoEntity>()
            .HasMany(p => p.Tarefas)
            .WithOne(t => t.Projeto)
            .HasForeignKey(t => t.ProjetoId);

        modelBuilder.Entity<TarefaEntity>()
            .Property(t => t.Status)
            .HasConversion(
                v => EnumHelper.GetEnumDescription(v),
                v => EnumHelper.GetEnumFromDescription<StatusTarefa>(v) 
            )
            .HasMaxLength(20)
            .IsRequired();

        modelBuilder.Entity<TarefaEntity>()
            .Property(t => t.Prioridade)
            .HasConversion(
                v => EnumHelper.GetEnumDescription(v),
                v => EnumHelper.GetEnumFromDescription<PrioridadeTarefa>(v)
            )
            .HasMaxLength(20)
            .IsRequired();

        modelBuilder.Entity<TarefaEntity>()
            .Property(t => t.Titulo).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<TarefaEntity>()
            .Property(t => t.Descricao).IsRequired().HasMaxLength(500);
        modelBuilder.Entity<TarefaEntity>()
            .Property(t => t.DataVencimento).IsRequired();
    }
}