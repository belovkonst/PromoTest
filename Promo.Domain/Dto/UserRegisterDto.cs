namespace Promo.Domain.Dto;

public record UserRegisterDto(string Login, string Pass, string PassConfirmation, int Country, int Province);
