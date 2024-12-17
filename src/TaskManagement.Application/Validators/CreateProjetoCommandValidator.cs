using TaskManagement.Application.Commands.Projetos;

namespace TaskManagement.Application.Validators;

public class UpsertProjetoCommandValidator : AbstractValidator<UpsertProjetoCommand>
{
    public UpsertProjetoCommandValidator()
    {
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("O nome do projeto é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do projeto não pode exceder 100 caracteres.");

        RuleFor(c => c.Descricao)
            .MaximumLength(500).WithMessage("A descrição não pode exceder 500 caracteres.");
    }
}