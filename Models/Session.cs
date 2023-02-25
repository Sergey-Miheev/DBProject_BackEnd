using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Session
{
    public int IdSession { get; set; }

    public int IdHall { get; set; }

    public int IdFilm { get; set; }

    public DateTime DateTime { get; set; }

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    public virtual Film IdFilmNavigation { get; set; } = null!;

    public virtual Hall IdHallNavigation { get; set; } = null!;
}
