using FluentValidation;

namespace Chat.Application.Commands.CreateChatSession;

public class CreateChatSessionCommandValidator : AbstractValidator<CreateChatSessionCommand>
{
    public CreateChatSessionCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
