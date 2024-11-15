﻿using Promo.Domain.Interfaces;

namespace Promo.Domain.Entities;

public class Province
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int CountryId { get; set; }
    public Country Country { get; set; }
}
