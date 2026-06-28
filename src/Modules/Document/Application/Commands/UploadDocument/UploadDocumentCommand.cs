using MediatR;
using SharedKernel.Domain;

namespace Document.Application.Commands.UploadDocument;

public record UploadDocumentCommand(
    string FileName,
    string ContentType,
    long FileSize,
    string StoragePath,
    Guid WorkspaceId,
    Guid UploadedBy) : IRequest<Result<DocumentResponse>>;

public record DocumentResponse(Guid Id, string Name, string ContentType, long FileSize, Guid WorkspaceId, Document.Domain.DocumentStatus Status, DateTime CreatedAt);
