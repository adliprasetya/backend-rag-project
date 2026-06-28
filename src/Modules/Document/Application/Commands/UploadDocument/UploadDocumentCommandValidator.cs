using FluentValidation;

namespace Document.Application.Commands.UploadDocument;

public class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
{
    public UploadDocumentCommandValidator()
    {
        RuleFor(x => x.FileName).NotEmpty().MaximumLength(500);
        RuleFor(x => x.FileSize).GreaterThan(0).WithMessage("File is empty");
        RuleFor(x => x.FileSize).LessThanOrEqualTo(50 * 1024 * 1024).WithMessage("File exceeds 50MB limit");
        RuleFor(x => x.WorkspaceId).NotEmpty();
        RuleFor(x => x.UploadedBy).NotEmpty();
    }
}
