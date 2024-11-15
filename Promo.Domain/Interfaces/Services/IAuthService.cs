using Promo.Domain.Dto;
using Promo.Domain.Results;

namespace Promo.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<Result> CheckUserParams(UserCheckDto dto);

    Task<Result<UserDto>> Register(UserRegisterDto dto);
}