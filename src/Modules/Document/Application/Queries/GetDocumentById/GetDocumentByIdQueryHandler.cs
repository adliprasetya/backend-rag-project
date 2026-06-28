using Document.Application.Commands.UploadDocument;
using MediatR;
using SharedKernel.Domain;

namespace Document.Application.Queries.GetDocumentById;

public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, Result<DocumentResponse>>
{
    private readonly IDocumentRepository _documentRepo;

    public GetDocumentByIdQueryHandler(IDocumentRepository documentRepo)
    {
        _documentRepo = documentRepo;
    }

    public async Task<Result<DocumentResponse>> Handle(GetDocumentByIdQuery request, CancellationToken ct)
    {
        var document = await _documentRepo.GetByIdAsync(request.Id, ct);
        if (document is null)
            return Result.Failure<DocumentResponse>(Error.NotFound("Document not found"));

        return Result.Success(new DocumentResponse(
            document.Id, document.Name, document.ContentType, document.FileSize,
            document.WorkspaceId, document.Status, document.CreatedAt));
    }
}
