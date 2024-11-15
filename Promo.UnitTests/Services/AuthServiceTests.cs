using Moq;
using Promo.Application.Services;
using Promo.Application.Resources;
using Promo.Domain.Dto;
using Promo.Domain.Entities;
using Promo.Domain.Enums;
using Promo.Domain.Interfaces.Repositories;

namespace Promo.UnitTests.Services;

public class AuthServiceTests : ServiceTestsBase
{
    private readonly Mock<IBaseRepository<User>> _userRepositoryMock;
    private readonly Mock<IBaseRepository<Country>> _countryRepositoryMock;
    private readonly Mock<IBaseRepository<Province>> _provinceRepositoryMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IBaseRepository<User>>();
        _countryRepositoryMock = new Mock<IBaseRepository<Country>>();
        _provinceRepositoryMock = new Mock<IBaseRepository<Province>>();
        _authService = new AuthService(_userRepositoryMock.Object, _countryRepositoryMock.Object, _provinceRepositoryMock.Object);
    }

    [Fact]
    public async Task CheckUserParams_WhenEmailNotValid()
    {
        // Arrange
        var dto = new UserCheckDto ("wrong-email", "123qwe", "123qwe" );

        var mockDbSet = CreateMockDbSet(new List<User>().AsQueryable());
        _userRepositoryMock.Setup(repo => repo.GetAll()).Returns(mockDbSet.Object);

        // Act
        var result = await _authService.CheckUserParams(dto);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal((int)ErrorCodes.EmailNotValid, result.ErrorCode);
        Assert.Equal(ErrorrTexts.EmailNotValid, result.ErrorText);
    }

    [Fact]
    public async Task CheckUserParams_WhenPasswordNotMetRequirements()
    {
        // Arrange
        var dto = new UserCheckDto("right-email@mail.com", "qwe", "qwe");

        var mockDbSet = CreateMockDbSet(new List<User>().AsQueryable());
        _userRepositoryMock.Setup(repo => repo.GetAll()).Returns(mockDbSet.Object);

        // Act
        var result = await _authService.CheckUserParams(dto);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal((int)ErrorCodes.PasswordNotMetRequirements, result.ErrorCode);
        Assert.Equal(ErrorrTexts.PasswordNotMetRequirements, result.ErrorText);
    }


    [Fact]
    public async Task CheckUserParams_WhenPasswordDoNotMatch()
    {
        // Arrange
        var dto = new UserCheckDto("right-email@mail.com", "123qwe", "qwe");
        
        var mockDbSet = CreateMockDbSet(new List<User>().AsQueryable());
        _userRepositoryMock.Setup(repo => repo.GetAll()).Returns(mockDbSet.Object);

        // Act
        var result = await _authService.CheckUserParams(dto);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal((int)ErrorCodes.PasswordsDoNotMatch, result.ErrorCode);
        Assert.Equal(ErrorrTexts.PasswordsDoNotMatch, result.ErrorText);
    }

    [Fact]
    public async Task CheckUserParams_WhenUserAlreadyExitsts()
    {
        // Arrange
        var dto = new UserCheckDto("right-email@mail.com", "123qwe", "123qwe");
        var existingUser = new User { Login = "right-email@mail.com" };
        
        var mockDbSet = CreateMockDbSet(new List<User> { existingUser }.AsQueryable());
        _userRepositoryMock.Setup(repo => repo.GetAll()).Returns(mockDbSet.Object);

        // Act
        var result = await _authService.CheckUserParams(dto);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal((int)ErrorCodes.UserAlreadyExitsts, result.ErrorCode);
        Assert.Equal(ErrorrTexts.UserAlreadyExitsts, result.ErrorText);
    }

    [Fact]
    public async Task CheckUserParams_WhenPasswordDoNotMatch2()
    {
        // Arrange
        var dto = new UserCheckDto("right-email@mail.com", "123qwe", "123qwe");
        
        var mockDbSet = CreateMockDbSet(new List<User>().AsQueryable());
        _userRepositoryMock.Setup(repo => repo.GetAll()).Returns(mockDbSet.Object);

        // Act
        var result = await _authService.CheckUserParams(dto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Null(result.ErrorCode);
        Assert.Null(result.ErrorText);
    }
}
