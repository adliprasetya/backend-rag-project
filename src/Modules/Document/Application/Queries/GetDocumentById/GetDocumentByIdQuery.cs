using Document.Application.Commands.UploadDocument;
using MediatR;
using SharedKernel.Domain;

namespace Document.Application.Queries.GetDocumentById;

public record GetDocumentByIdQuery(Guid Id) : IRequest<Result<DocumentResponse>>;
