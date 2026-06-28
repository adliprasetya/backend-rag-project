using FluentValidation;

namespace Chat.Application.Commands.SendMessage;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty();
        RuleFor(x => x.Content).NotEmpty().MaximumLength(100000);
        RuleFor(x => x.UserId).NotEmpty();
    }
}
