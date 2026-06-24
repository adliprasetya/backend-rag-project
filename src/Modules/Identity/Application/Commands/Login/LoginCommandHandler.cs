using Identity.Application;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Domain;

namespace Identity.Application.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenResult>>
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(IUserRepository userRepo, IJwtService jwtService)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
    }

    public async Task<Result<TokenResult>> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _userRepo.FindByEmailAsync(request.Email, ct);
        if (user is null)
            return Result.Failure<TokenResult>(Error.Unauthorized("Invalid credentials"));

        var verifier = new PasswordHasher<object>();
        var result = verifier.VerifyHashedPassword(new object(), user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
            return Result.Failure<TokenResult>(Error.Unauthorized("Invalid credentials"));

        var token = _jwtService.GenerateToken(user.Id, user.Email);
        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpiresAt = token.ExpiresAt.AddDays(7);
        _userRepo.Update(user);

        return Result.Success(token);
    }
}
