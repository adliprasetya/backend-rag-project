using Identity.Application;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel;
using SharedKernel.Domain;

namespace Identity.Application.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<TokenResult>>
{
    private readonly IUserRepository _userRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(
        IUserRepository userRepo,
        IUnitOfWork unitOfWork,
        IJwtService jwtService)
    {
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<Result<TokenResult>> Handle(RegisterCommand request, CancellationToken ct)
    {
        var passwordHash = new PasswordHasher<object>().HashPassword(new object(), request.Password);
        var user = User.Create(request.Name, request.Email, passwordHash);

        await _userRepo.AddAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        var token = _jwtService.GenerateToken(user.Id, user.Email);
        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpiresAt = token.ExpiresAt.AddDays(7);
        _userRepo.Update(user);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success(token);
    }
}
