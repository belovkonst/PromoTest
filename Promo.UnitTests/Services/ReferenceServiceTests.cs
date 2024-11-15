using Microsoft.EntityFrameworkCore;
using Moq;
using Promo.Application.Services;
using Promo.Domain.Entities;
using Promo.Domain.Interfaces.Repositories;
using Promo.UnitTests.Helpers;

namespace Promo.UnitTests.Services;

public class ReferenceServiceTests : ServiceTestsBase
{
    private readonly Mock<IBaseRepository<Country>> _countryRepositoryMock;
    private readonly Mock<IBaseRepository<Province>> _provinceRepositoryMock;
    private readonly ReferenceService _referenceService;

    public ReferenceServiceTests()
    {
        _countryRepositoryMock = new Mock<IBaseRepository<Country>>();
        _provinceRepositoryMock = new Mock<IBaseRepository<Province>>();
        _referenceService = new ReferenceService(_countryRepositoryMock.Object, _provinceRepositoryMock.Object);
    }

    [Fact]
    public async Task GetCountries_WhenDataExist()
    {
        // Arrange
        var countryList = new List<Country>
        {
                new Country { Id = 1, Name = "USA" },
                new Country { Id = 2, Name = "Canada" },
                new Country { Id = 3, Name = "Germany" },
                new Country { Id = 4, Name = "Australia" }
        }.AsQueryable();
    
        var mockDbSet = CreateMockDbSet(countryList);
        _countryRepositoryMock.Setup(repo => repo.GetAll()).Returns(mockDbSet.Object);

        // Act
        var result = await _referenceService.GetCountries();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Data.Count());
        Assert.Contains(result.Data, c => c.Name == "USA");
        Assert.Contains(result.Data, c => c.Name == "Canada");
        Assert.Contains(result.Data, c => c.Name == "Germany");
        Assert.Contains(result.Data, c => c.Name == "Australia");
    }


    [Fact]
    public async Task GetCountries_WhenDataNotExist()
    {
        // Arrange
        var countryList = new List<Country>().AsQueryable();

        var mockDbSet = CreateMockDbSet(countryList);
        _countryRepositoryMock.Setup(repo => repo.GetAll()).Returns(mockDbSet.Object);

        // Act
        var result = await _referenceService.GetCountries();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Data);
        Assert.DoesNotContain(result.Data, c => c.Name == "USA");
    }

    [Fact]
    public async Task GetProvincesByCountry_WhenDataExist()
    {
        // Arrange
        var provinceList = new List<Province>
            {
                new Province { Id = 1, Name = "California", CountryId = 1 },
                new Province { Id = 2, Name = "Texas", CountryId = 1 }
            }.AsQueryable();

        var mockDbSet = CreateMockDbSet(provinceList);
        _provinceRepositoryMock.Setup(repo => repo.GetAll()).Returns(mockDbSet.Object);

        // Act
        var result = await _referenceService.GetProvincesByCountry(1);

        // Assert
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count());
        Assert.Contains(result.Data, p => p.Name == "California" && p.CountryId == 1);
        Assert.Contains(result.Data, p => p.Name == "Texas" && p.CountryId == 1);
    }

    [Fact]
    public async Task GetProvincesByCountry_WhenDataNotExist()
    {
        // Arrange
        var provinceList = new List<Province>().AsQueryable();

        var mockDbSet = CreateMockDbSet(provinceList);
        _provinceRepositoryMock.Setup(repo => repo.GetAll()).Returns(mockDbSet.Object);

        // Act
        var result = await _referenceService.GetProvincesByCountry(1);

        // Assert
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);
    }
}