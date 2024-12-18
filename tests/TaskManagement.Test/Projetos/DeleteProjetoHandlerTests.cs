namespace TaskManagement.Test.Projetos;

public class DeleteProjetoHandlerTests
{
    private readonly Mock<IRepository<ProjetoEntity>> _projetoRepositoryMock;
    private readonly Mock<IRepository<TarefaEntity>> _tarefaRepositoryMock;
    private readonly Mock<IHistoricoAtualizacaoService> _historicoServiceMock;
    private readonly DeleteProjetoCommandHandler _handler;

    public DeleteProjetoHandlerTests()
    {
        _projetoRepositoryMock = new Mock<IRepository<ProjetoEntity>>();
        _tarefaRepositoryMock = new Mock<IRepository<TarefaEntity>>();
        _historicoServiceMock = new Mock<IHistoricoAtualizacaoService>();
        _handler = new DeleteProjetoCommandHandler(_projetoRepositoryMock.Object, _historicoServiceMock.Object);
    }

    [Fact(DisplayName = "Remover Projeto com Tarefas pendentes")]
    public async Task TestRemoverProjetoComTarefasPendentes()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var projeto = new ProjetoEntity
        {
            Id = projetoId,
            Nome = "Projeto com Tarefas Pendentes",
            Descricao = "Projeto teste"
        };

        var tarefasPendentes = new List<TarefaEntity>
        {
            new TarefaEntity 
            { 
                Id = Guid.NewGuid(),
                Titulo = "Tarefa Pendente",
                Descricao = "Descrevendo Tarefa Pendente",
                Status = StatusTarefa.Pendente,
                Projeto = projeto
            }
        };

        _projetoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(projeto);

        _tarefaRepositoryMock.Setup(repo => repo.GetSpecificAsync(It.IsAny<Expression<Func<TarefaEntity, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefasPendentes);

        var command = new DeleteProjetoCommand { Id = projetoId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _projetoRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.False(result.Success);
        Assert.Equal("Não é possível deletar o Projeto. Existem tarefas pendentes associadas.", result.Message);
    }

    [Fact(DisplayName = "Remover Projeto sem Tarefas pendentes")]
    public async Task TestRemoverProjetoSemTarefasPendentes()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var projeto = new ProjetoEntity
        {
            Id = projetoId,
            Nome = "Projeto sem Tarefas Pendentes",
            Descricao = "Projeto teste"
        };

        var tarefasPendentes = new List<TarefaEntity>();

        _projetoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(projeto);

        _tarefaRepositoryMock.Setup(repo => repo.GetSpecificAsync(It.IsAny<Expression<Func<TarefaEntity, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefasPendentes);

        var command = new DeleteProjetoCommand { Id = projetoId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _projetoRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result.Success);
    }
}
