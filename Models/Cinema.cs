using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Cinema
{
    public int IdCinema { get; set; }

    public string Name { get; set; } = null!;

    public string CityName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Hall> Halls { get; } = new List<Hall>();
}
