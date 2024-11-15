using Promo.Domain.Dto;
using Promo.Domain.Results;

namespace Promo.Domain.Interfaces.Services;

public interface IReferenceService
{
    Task<Result<IEnumerable<CountryDto>>> GetCountries();

    Task<Result<IEnumerable<ProvinceDto>>> GetProvincesByCountry(int countryId);
}
