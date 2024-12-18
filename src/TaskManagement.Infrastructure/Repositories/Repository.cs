namespace TaskManagement.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly TaskDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(TaskDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    #region Custom

    public async Task<IEnumerable<TarefaEntity>> GetObjectsWithAnotherAsync(Guid id, CancellationToken cancellationToken)
    {
        var entities = await _context.Tarefas
            .Where(p => p.ProjetoId == id)
            .Include(p => p.Projeto)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entities.Count != 0 ? entities : [];
    }

    public async Task<List<ProjetoEntity>> GetProjetoCompleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var query = _context.Projetos
            .Include(p => p.Tarefas)
            .ThenInclude(p => p.Comentarios)
            .AsNoTracking();

        if (id != Guid.Empty)
        {
            query = query.Where(p => p.Id == id);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<ComentarioEntity>> GetComentariosPorTarefasAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Comentarios
            .Where(p => p.TarefaId == id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<RelatorioDesempenhoDto>> ObterRelatorioDesempenhoAsync(int ultimosDias, CancellationToken cancellationToken)
    {
        var relatorio = await _context.Set<TarefaEntity>()
            .Where(t => t.Status == StatusTarefa.Concluida && t.DataVencimento >= DateTime.UtcNow.AddDays(ultimosDias))
            .GroupBy(t => new { t.ProjetoId, t.Projeto.Nome })
            .Select(g => new RelatorioDesempenhoDto
            {
                ProjetoId = g.Key.ProjetoId,
                NomeProjeto = g.Key.Nome,
                TarefasConcluidas = g.Count(),
                MediaTarefasPorDia = g.Count() / 30.0
            })
            .OrderByDescending(r => r.TarefasConcluidas)
            .ToListAsync(cancellationToken);

        return relatorio;
    }

    public async Task<List<TarefaEntity>> GetTarefasPorProjetoAsync(Guid? id, CancellationToken cancellationToken)
    {
        if (id.HasValue && id != Guid.Empty)
        {
            // Busca tarefas vinculadas ao Projeto específico
            return await _context.Tarefas
                .AsNoTracking()
                .Where(t => t.ProjetoId == id.Value)
                .Include(t => t.Projeto)
                .ToListAsync(cancellationToken);
        }

        // Busca todas as tarefas
        return await _context.Tarefas
            .AsNoTracking()
            .Include(t => t.Projeto)
            .ToListAsync(cancellationToken);
    }

    #endregion

    #region Generic

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        return entity ?? throw new InvalidOperationException("Entity not found.");
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetSpecificAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        var query = _dbSet.AsNoTracking();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync(cancellationToken);
    }


    public async Task<Guid> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var idProperty = typeof(TEntity).GetProperty("Id");
        if (idProperty == null)
        {
            return Guid.Empty;
        }

        var value = idProperty.GetValue(entity);
        return value is Guid guidValue ? guidValue : Guid.Empty;

    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var entityInContext = await _dbSet
            .FirstOrDefaultAsync(e => e.Id == entity.Id, cancellationToken);

        if (entityInContext != null)
        {
            _context.Entry(entityInContext).CurrentValues.SetValues(entity);
        }
        else
        {
            _dbSet.Update(entity);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        bool result = false;
        var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            result = true;
        }
        return result;
    }

    #endregion

    #region Seed

    public static void Seed(TaskDbContext context)
    {
        var projetos = new List<ProjetoEntity>
        {
            new() {
                Id = Guid.NewGuid(),
                Nome = "Desenvolvimento de Sistema de Gestão",
                Descricao = "Projeto para desenvolver um sistema de gestão para a empresa de TI",
                DataCriacao = DateTime.UtcNow
            },
            new() {
                Id = Guid.NewGuid(),
                Nome = "Implementação de API Restful",
                Descricao = "Projeto para criar e implementar uma API Restful utilizando .NET Core",
                DataCriacao = DateTime.UtcNow
            },
            new() {
                Id = Guid.NewGuid(),
                Nome = "Automação de Testes de Software",
                Descricao = "Projeto de automação de testes para melhorar a cobertura de testes",
                DataCriacao = DateTime.UtcNow
            },
            new() {
                Id = Guid.NewGuid(),
                Nome = "Desenvolvimento de Aplicativo Mobile",
                Descricao = "Projeto para desenvolver um aplicativo mobile para iOS e Android",
                DataCriacao = DateTime.UtcNow
            },
            new() {
                Id = Guid.NewGuid(),
                Nome = "Integração de Sistemas ERP",
                Descricao = "Projeto para integrar diferentes módulos de um sistema ERP",
                DataCriacao = DateTime.UtcNow
            }
        };

        context.Projetos.AddRange(projetos);
        context.SaveChanges();

        foreach (var projeto in projetos)
        {
            var tarefas = new List<TarefaEntity>
            {
                new() {
                    Id = Guid.NewGuid(),
                    Titulo = "Análise de Requisitos",
                    Descricao = "Realizar levantamento de requisitos com o cliente",
                    DataVencimento = DateTime.UtcNow.AddDays(7),
                    Status = StatusTarefa.Pendente,
                    Prioridade = PrioridadeTarefa.Alta,
                    ProjetoId = projeto.Id,
                    Projeto = projeto
                },
                new() {
                    Id = Guid.NewGuid(),
                    Titulo = "Desenvolvimento Backend",
                    Descricao = "Desenvolver API para cadastro de usuários",
                    DataVencimento = DateTime.UtcNow.AddDays(14),
                    Status = StatusTarefa.Pendente,
                    Prioridade = PrioridadeTarefa.Media,
                    ProjetoId = projeto.Id,
                    Projeto = projeto
                },
                new() {
                    Id = Guid.NewGuid(),
                    Titulo = "Testes de Unidade",
                    Descricao = "Escrever testes unitários para a API",
                    DataVencimento = DateTime.UtcNow.AddDays(10),
                    Status = StatusTarefa.Pendente,
                    Prioridade = PrioridadeTarefa.Media,
                    ProjetoId = projeto.Id,
                    Projeto = projeto
                },
                new() {
                    Id = Guid.NewGuid(),
                    Titulo = "Revisão de Código",
                    Descricao = "Revisar o código desenvolvido para garantir qualidade",
                    DataVencimento = DateTime.UtcNow.AddDays(5),
                    Status = StatusTarefa.Pendente,
                    Prioridade = PrioridadeTarefa.Alta,
                    ProjetoId = projeto.Id,
                    Projeto = projeto
                },
                new() {
                    Id = Guid.NewGuid(),
                    Titulo = "Desenvolvimento Frontend",
                    Descricao = "Desenvolver a interface do usuário",
                    DataVencimento = DateTime.UtcNow.AddDays(7),
                    Status = StatusTarefa.Pendente,
                    Prioridade = PrioridadeTarefa.Media,
                    ProjetoId = projeto.Id,
                    Projeto = projeto
                }
            };

            context.Tarefas.AddRange(tarefas);
            context.SaveChanges();

            foreach (var tarefa in tarefas)
            {
                var random = new Random();
                var numberOfComentarios = random.Next(0, 4);

                for (int i = 0; i < numberOfComentarios; i++)
                {
                    context.Comentarios.Add(new ComentarioEntity
                    {
                        Id = Guid.NewGuid(),
                        TarefaId = tarefa.Id,
                        Comentario = $"Comentário aleatório {i + 1} para a tarefa {tarefa.Titulo}",
                        DataCriacao = DateTime.UtcNow
                    });
                }
            }
        }
        context.SaveChanges();
    }

    #endregion
}