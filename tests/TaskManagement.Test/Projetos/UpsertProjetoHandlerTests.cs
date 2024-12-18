namespace TaskManagement.Test.Projetos;

public class UpsertProjetoHandlerTests
{
    private readonly Mock<IRepository<ProjetoEntity>> _projetoRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IHistoricoAtualizacaoService> _historicoServiceMock;
    private readonly UpsertProjetoCommandHandler _handler;

    public UpsertProjetoHandlerTests()
    {
        _projetoRepositoryMock = new Mock<IRepository<ProjetoEntity>>();
        _mapperMock = new Mock<IMapper>();
        _historicoServiceMock = new Mock<IHistoricoAtualizacaoService>();
        _handler = new UpsertProjetoCommandHandler(_projetoRepositoryMock.Object, _historicoServiceMock.Object, _mapperMock.Object);
    }

    [Fact(DisplayName = "Inserir Projeto")]
    public async Task TestInserirProjeto()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var command = new UpsertProjetoCommand
        {
            Id = Guid.Empty, 
            Nome = "Novo Projeto",
            Descricao = "Descrição do novo projeto"
        };

        var projetoEntity = new ProjetoEntity
        {
            Id = projetoId,
            Nome = command.Nome,
            Descricao = command.Descricao,
            DataCriacao = DateTime.UtcNow
        };

        _mapperMock.Setup(m => m.Map<ProjetoEntity>(It.IsAny<UpsertProjetoCommand>()))
            .Returns(projetoEntity);

        _projetoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ProjetoEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(projetoEntity.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _projetoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ProjetoEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result.Success);
    }

    [Fact(DisplayName = "Atualizar Projeto")]
    public async Task TestAtualizarProjeto()
    {
        // Arrange
        var projetoId = Guid.NewGuid();
        var command = new UpsertProjetoCommand
        {
            Id = projetoId,
            Nome = "Projeto Atualizado",
            Descricao = "Descrição do projeto atualizado"
        };

        var projetoEntity = new ProjetoEntity
        {
            Id = projetoId,
            Nome = "Projeto Antigo",
            Descricao = "Descrição antiga do projeto",
            DataCriacao = DateTime.UtcNow
        };

        _mapperMock.Setup(m => m.Map<ProjetoEntity>(It.IsAny<UpsertProjetoCommand>()))
            .Returns(projetoEntity);

        _projetoRepositoryMock.Setup(repo => repo.GetByIdAsync(projetoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(projetoEntity);

        _projetoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ProjetoEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(projetoEntity.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _projetoRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ProjetoEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result.Success);
    }
}
