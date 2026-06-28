using Document.Application.Commands.UploadDocument;
using MediatR;
using SharedKernel.Domain;

namespace Document.Application.Queries.GetDocuments;

public class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQuery, Result<List<DocumentResponse>>>
{
    private readonly IDocumentRepository _documentRepo;

    public GetDocumentsQueryHandler(IDocumentRepository documentRepo)
    {
        _documentRepo = documentRepo;
    }

    public async Task<Result<List<DocumentResponse>>> Handle(GetDocumentsQuery request, CancellationToken ct)
    {
        var documents = await _documentRepo.GetByWorkspaceAsync(request.WorkspaceId, ct);

        var response = documents.Select(d => new DocumentResponse(
            d.Id, d.Name, d.ContentType, d.FileSize, d.WorkspaceId, d.Status, d.CreatedAt)).ToList();

        return Result.Success(response);
    }
}
