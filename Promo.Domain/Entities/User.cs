using Promo.Domain.Interfaces;

namespace Promo.Domain.Entities;

public class User : IAuditable
{
    public long Id { get; set; }
    public string Login {  get; set; }
    public string Password { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }

    public int CountryId { get; set; }
    public Country Country { get; set; }

    public int ProvinceId { get; set; }
    public Province Province { get; set; }
}
