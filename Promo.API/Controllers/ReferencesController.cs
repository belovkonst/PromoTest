using Microsoft.AspNetCore.Mvc;
using Promo.Domain.Dto;
using Promo.Domain.Interfaces.Services;
using Promo.Domain.Results;

namespace Promo.Api.Controllers;

[ApiController]
[Route("api/references")]
public class ReferencesController : ControllerBase
{
    private readonly IReferenceService _referenceService;

    public ReferencesController(IReferenceService referenceService)
    {
        _referenceService = referenceService;
    }

    [HttpGet("countries")]
    public async Task<ActionResult<Result<IEnumerable<CountryDto>>>> GetCountries()
    {
        var response = await _referenceService.GetCountries();
        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpGet("provinces")]
    public async Task<ActionResult<Result<IEnumerable<ProvinceDto>>>> GetProvinces([FromQuery] int countryId)
    {
        var response = await _referenceService.GetProvincesByCountry(countryId);
        if (response.Success)
            return Ok(response);

        return BadRequest(response);
    }
}
