using Document.Domain;
using MediatR;
using SharedKernel;
using SharedKernel.Domain;

namespace Document.Application.Commands.UploadDocument;

public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, Result<DocumentResponse>>
{
    private readonly IDocumentRepository _documentRepo;
    private readonly IUnitOfWork _unitOfWork;

    public UploadDocumentCommandHandler(IDocumentRepository documentRepo, IUnitOfWork unitOfWork)
    {
        _documentRepo = documentRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DocumentResponse>> Handle(UploadDocumentCommand request, CancellationToken ct)
    {
        var document = Domain.Document.Create(
            request.FileName,
            request.ContentType,
            request.FileSize,
            request.StoragePath,
            request.WorkspaceId,
            request.UploadedBy);

        await _documentRepo.AddAsync(document, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success(new DocumentResponse(
            document.Id,
            document.Name,
            document.ContentType,
            document.FileSize,
            document.WorkspaceId,
            document.Status,
            document.CreatedAt));
    }
}
