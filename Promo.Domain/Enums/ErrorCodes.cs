namespace Promo.Domain.Enums;

public enum ErrorCodes
{
    EmailNotValid = 1,
    PasswordNotMetRequirements = 2,
    PasswordsDoNotMatch = 3,
    UserAlreadyExitsts = 4,

    CountryNotFound = 5,
    ProvinceNotFound = 6,

    ErrorWhileCreatingUser = 7
}
