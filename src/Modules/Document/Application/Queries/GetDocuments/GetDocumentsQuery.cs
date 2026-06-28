using Document.Application.Commands.UploadDocument;
using MediatR;
using SharedKernel.Domain;

namespace Document.Application.Queries.GetDocuments;

public record GetDocumentsQuery(Guid WorkspaceId) : IRequest<Result<List<DocumentResponse>>>;
