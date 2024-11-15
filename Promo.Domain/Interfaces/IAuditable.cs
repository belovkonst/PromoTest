namespace Promo.Domain.Interfaces;

public interface IAuditable
{
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
}
