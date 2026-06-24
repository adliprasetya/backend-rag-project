using MediatR;
using SharedKernel.Domain;

namespace Identity.Application.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<TokenResult>>;
