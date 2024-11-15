using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Promo.Domain.Dto;
using Promo.Domain.Interfaces.Services;
using Promo.Domain.Results;

namespace Promo.Api.Controllers;

[ApiController]
[Route("api/users")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("checkUserParams")]
    public async Task<ActionResult<Result>> CheckUserParams([FromBody] UserCheckDto dto)
    {
        var response = await _authService.CheckUserParams(dto);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpPost("registerUser")]
    public async Task<ActionResult<Result<UserDto>>> Register([FromBody] UserRegisterDto dto)
    {
        var response = await _authService.Register(dto);

        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }
}
