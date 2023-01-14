using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Hall
{
    public int IdHall { get; set; }

    public int IdCinema { get; set; }

    public short Number { get; set; }

    public short Type { get; set; }

    public short Capacity { get; set; }

    public virtual Cinema IdCinemaNavigation { get; set; } = null!;

    public virtual ICollection<Place> Places { get; } = new List<Place>();

    public virtual ICollection<Session> Sessions { get; } = new List<Session>();
}
