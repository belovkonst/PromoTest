using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

using Promo.Domain.Dto;
using Promo.Domain.Entities;
using Promo.Domain.Interfaces.Repositories;
using Promo.Domain.Interfaces.Services;
using Promo.Domain.Results;
using Promo.Domain.Enums;
using Promo.Application.Resources;



namespace Promo.Application.Services;

public class AuthService : IAuthService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<Country> _countryRepository;
    private readonly IBaseRepository<Province> _provinceRepository;


    public AuthService(IBaseRepository<User> userRepository, IBaseRepository<Country> countryRepository, IBaseRepository<Province> provinceRepository)
    {
        _userRepository = userRepository;
        _countryRepository = countryRepository;
        _provinceRepository = provinceRepository;
    }


    public async Task<Result> CheckUserParams(UserCheckDto dto)
    {
        var result = new Result();

        if (!IsValidEmail(dto.Login.Trim()))
        {
            result.ErrorText = ErrorrTexts.EmailNotValid;
            result.ErrorCode = (int)ErrorCodes.EmailNotValid;

            return result;
        }

        if (!IsValidPassword(dto.Pass.Trim()))
        {
            result.ErrorText = ErrorrTexts.PasswordNotMetRequirements;
            result.ErrorCode = (int)ErrorCodes.PasswordNotMetRequirements;

            return result;
        }

        if (dto.Pass != dto.PassConfirmation)
        {
            result.ErrorText = ErrorrTexts.PasswordsDoNotMatch;
            result.ErrorCode = (int)ErrorCodes.PasswordsDoNotMatch;

            return result;
        }

        var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Login == dto.Login.ToLower().Trim());
        if (user != null) 
        {
            result.ErrorText = ErrorrTexts.UserAlreadyExitsts;
            result.ErrorCode = (int)ErrorCodes.UserAlreadyExitsts;

            return result;
        }

        return result;
    }

    public async Task<Result<UserDto>> Register(UserRegisterDto dto)
    {
        var result = new Result<UserDto>();

        var userCheckDto = new UserCheckDto(dto.Login, dto.Pass, dto.PassConfirmation);
        var checkResult = await CheckUserParams(userCheckDto);
        if (!checkResult.Success)
        {
            result.ErrorText = checkResult.ErrorText;
            result.ErrorCode = checkResult.ErrorCode;
            return result;
        }

        var country = await _countryRepository.GetAll().FirstOrDefaultAsync(c => c.Id == dto.Country);
        if (country == null)
        {
            result.ErrorText = string.Format(ErrorrTexts.CountryNotFound, dto.Country);
            result.ErrorCode = (int)ErrorCodes.CountryNotFound;

            return result;
        }

        var province = await _provinceRepository.GetAll().FirstOrDefaultAsync(p => p.Id == dto.Province && p.CountryId == dto.Country);
        if (province == null)
        {
            result.ErrorText = string.Format(ErrorrTexts.ProvinceNotFound, dto.Province, dto.Country);
            result.ErrorCode = (int)ErrorCodes.ProvinceNotFound;

            return result;
        }

        var hashedPass = HashPassword(dto.Pass);
        try
        {
            var user = new User()
            {
                Login = dto.Login.ToLower().Trim(),
                Password = hashedPass,
                Country = country,
                Province = province,
            };

            await _userRepository.CreateAsync(user);
            await _userRepository.SaveChangesAsync();

            result.Data = new UserDto(user.Id);
        }
        catch (Exception ex)
        {
            result.ErrorText = ex.Message;
            result.ErrorCode = (int)ErrorCodes.ErrorWhileCreatingUser;
        }

        return result;
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    private bool IsValidPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return false;

        if (password.Length < 2)
            return false;

        // Must contain min 1 digit and min 1 letter.
        string pattern = @"^(?=.*[A-Za-z])(?=.*\d).+$";
        return Regex.IsMatch(password, pattern);
    }

    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}