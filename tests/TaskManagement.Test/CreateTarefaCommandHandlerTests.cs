namespace TaskManagement.Test;

public class CreateTarefaCommandHandlerTests
{
    private readonly Mock<TaskDbContext> _mockContext;
    private readonly IMapper _mapper;

    public CreateTarefaCommandHandlerTests()
    {
        _mockContext = new Mock<TaskDbContext>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new TarefaProfile()));
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnGuid()
    {
        // Arrange
        var handler = new CreateTarefaCommandHandler(_mockContext.Object, _mapper);
        var command = new CreateTarefaCommand
        {
            ProjetoId = Guid.NewGuid(),
            Titulo = "Nova Tarefa",
            Descricao = "Descrição de teste",
            DataVencimento = DateTime.UtcNow.AddDays(1),
            Prioridade = "Alta"
        };

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.IsType<Guid>(result);
    }
}