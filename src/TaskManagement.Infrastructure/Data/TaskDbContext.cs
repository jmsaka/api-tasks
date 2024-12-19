namespace TaskManagement.Infrastructure.Data;

public class TaskDbContext : DbContext
{
    public required DbSet<ProjetoEntity> Projetos { get; set; }
    public required DbSet<TarefaEntity> Tarefas { get; set; }
    public required DbSet<ComentarioEntity> Comentarios { get; set; }
    public required DbSet<HistoricoAtualizacaoEntity> Historicos { get; set; }

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
        modelBuilder.Entity<ProjetoEntity>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.HasMany(p => p.Tarefas)
                .WithOne(t => t.Projeto)
                .HasForeignKey(t => t.ProjetoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TarefaEntity>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.HasMany(t => t.Comentarios)
                .WithOne(c => c.Tarefa)
                .HasForeignKey(c => c.TarefaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(t => t.Status)
                .HasConversion(
                    v => EnumHelper.GetEnumDescription(v),
                    v => EnumHelper.GetEnumFromDescription<StatusTarefa>(v)
                )
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(t => t.Prioridade)
                .HasConversion(
                    v => EnumHelper.GetEnumDescription(v),
                    v => EnumHelper.GetEnumFromDescription<PrioridadeTarefa>(v)
                )
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(t => t.Titulo)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(t => t.DataVencimento)
                .IsRequired();
        });

        modelBuilder.Entity<ComentarioEntity>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Comentario)
                .HasMaxLength(10000);
        });

        modelBuilder.Entity<HistoricoAtualizacaoEntity>(entity =>
        {
            entity.HasKey(h => h.Id);

            entity.Property(h => h.Entidade)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(h => h.ValorAntigo)
                .IsRequired()
                .HasMaxLength(4000);

            entity.Property(h => h.ValorNovo)
                .IsRequired()
                .HasMaxLength(4000);

            entity.Property(h => h.DataAlteracao)
                .IsRequired();

            entity.Property(h => h.UsuarioResponsavel)
                .IsRequired()
                .HasMaxLength(100);
        });
    }
}