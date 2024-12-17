namespace TaskManagement.Application.Validators;

public class UpsertComentarioCommandValidator : AbstractValidator<UpsertComentarioCommand>
{
    public UpsertComentarioCommandValidator()
    {
        RuleFor(c => c.Comentario)
            .MaximumLength(500).WithMessage("O comentário não pode exceder 500 caracteres.");
    }
}