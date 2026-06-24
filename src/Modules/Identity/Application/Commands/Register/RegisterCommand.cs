using MediatR;
using SharedKernel.Domain;

namespace Identity.Application.Commands.Register;

public record RegisterCommand(string Name, string Email, string Password) : IRequest<Result<TokenResult>>;
