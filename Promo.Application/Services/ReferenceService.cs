using Promo.Domain.Dto;
using Promo.Domain.Entities;
using Promo.Domain.Interfaces.Repositories;
using Promo.Domain.Interfaces.Services;
using Promo.Domain.Results;
using Microsoft.EntityFrameworkCore;

namespace Promo.Application.Services;

public class ReferenceService : IReferenceService
{
    private readonly IBaseRepository<Country> _countryRepository;
    private readonly IBaseRepository<Province> _provinceRepository;

    public ReferenceService(IBaseRepository<Country> countryRepository, IBaseRepository<Province> provinceRepository)
    {
        _countryRepository = countryRepository;
        _provinceRepository = provinceRepository;
    }

    public async Task<Result<IEnumerable<CountryDto>>> GetCountries()
    {
        var countries = await _countryRepository.GetAll()
            .Select(c => new CountryDto(c.Id, c.Name))
            .ToListAsync();
 
        return new Result<IEnumerable<CountryDto>> { Data = countries };
    }

    public async Task<Result<IEnumerable<ProvinceDto>>> GetProvincesByCountry(int countryId)
    {
        var provinces = await _provinceRepository.GetAll()
            .Where(p => p.CountryId == countryId)
            .Select(p => new ProvinceDto(p.Id, p.Name, p.CountryId))
            .ToListAsync();
        return new Result<IEnumerable<ProvinceDto>> { Data = provinces };
    }
}