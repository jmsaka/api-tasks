namespace TaskManagement.Application.Validators;

public class UpsertTarefaCommandValidator : AbstractValidator<UpsertTarefaCommand>
{
    private static readonly string[] PrioridadesValidas = Enum.GetValues(typeof(PrioridadeTarefa))
        .Cast<PrioridadeTarefa>()
        .Select(p => EnumHelper.GetEnumDescription(p))
        .ToArray();

    public UpsertTarefaCommandValidator()
    {
        RuleFor(t => t.Titulo)
            .NotEmpty().WithMessage("O título da tarefa é obrigatório.")
            .MaximumLength(100).WithMessage("O título não pode exceder 100 caracteres.");

        RuleFor(t => t.Descricao)
            .MaximumLength(500).WithMessage("A descrição não pode exceder 500 caracteres.");

        RuleFor(t => t.DataVencimento)
            .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("A data de vencimento deve ser no futuro.");

        RuleFor(t => t.Prioridade)
            .Must(p => PrioridadesValidas.Contains(EnumHelper.GetEnumDescription(p)))
            .WithMessage("A prioridade deve ser 'Baixa', 'Média' ou 'Alta'.");
    }
}