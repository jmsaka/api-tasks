namespace TaskManagement.Test;

public class CreateProjetoCommandHandlerTests
{
    private readonly Mock<TaskDbContext> _mockContext;
    private readonly IMapper _mapper;

    public CreateProjetoCommandHandlerTests()
    {
        _mockContext = new Mock<TaskDbContext>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new ProjetoProfile()));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnGuid()
    {
        // Arrange
        var handler = new CreateProjetoCommandHandler(_mockContext.Object, _mapper);
        var command = new CreateProjetoCommand { Nome = "Projeto Teste", Descricao = "Descrição Teste" };

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.IsType<Guid>(result);
    }
}