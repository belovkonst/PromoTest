using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Promo.Domain.Entities;
using Promo.Domain.Interfaces.Repositories;

namespace Promo.DAL.Data;

public class DBInitializer : IDBInitializer
{
    private readonly IBaseRepository<Country> _countryRepository;
    private readonly IBaseRepository<Province> _provinceRepository;
    private readonly ApplicationDbContext _applicationDbContext;

    public DBInitializer(IBaseRepository<Country> CountryRepository, IBaseRepository<Province> ProvinceRepository, ApplicationDbContext applicationDbContext)
    {
        _countryRepository = CountryRepository;
        _provinceRepository = ProvinceRepository;
        _applicationDbContext = applicationDbContext;

    }

    public void Initialize()
    {
        if (_applicationDbContext.Database.GetPendingMigrations().Count () > 0 )
        {
            _applicationDbContext.Database.Migrate();
        }

        if (!_countryRepository.Any())
        {
            var countriesSeed = new List<Country>
            {
                new Country { Name = "USA" },
                new Country { Name = "Canada" },
                new Country { Name = "Germany" },
                new Country { Name = "Australia" }
            };

            foreach (var country in countriesSeed)
            {
                _countryRepository.CreateAsync(country).GetAwaiter().GetResult();
            }
            _countryRepository.SaveChangesAsync().GetAwaiter().GetResult();
        }

        if (!_provinceRepository.Any())
        {
            var countriesRef = _countryRepository.GetAll();

            var usa = countriesRef.FirstOrDefault(c => c.Name == "USA");
            var canada = countriesRef.FirstOrDefault(c => c.Name == "Canada");
            var germany = countriesRef.FirstOrDefault(c => c.Name == "Germany");
            var australia = countriesRef.FirstOrDefault(c => c.Name == "Australia");

            var provincesSeed = new List<Province>
            {
                new Province { Name = "California", CountryId = usa.Id, Country = usa },
                new Province { Name = "Texas", CountryId = usa.Id, Country = usa },
                new Province { Name = "Ohio", CountryId = usa.Id, Country = usa },
                new Province { Name = "Ontario", CountryId = canada.Id, Country = canada },
                new Province { Name = "Quebec", CountryId = canada.Id, Country = canada },
                new Province { Name = "Bavaria", CountryId = germany.Id, Country = germany },
                new Province { Name = "North Rhine-Westphalia", CountryId = germany.Id, Country = germany },
                new Province { Name = "New South Wales", CountryId = australia.Id, Country = australia },
                new Province { Name = "Queensland", CountryId = australia.Id, Country = australia }
            };

            foreach (var province in provincesSeed)
                _provinceRepository.CreateAsync(province).GetAwaiter().GetResult();

            _provinceRepository.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
