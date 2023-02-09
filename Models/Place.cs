using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Place
{
    public int IdPlace { get; set; }

    public int IdHall { get; set; }

    public short Row { get; set; }

    public short SeatNumber { get; set; }

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    public virtual Hall IdHallNavigation { get; set; } = null!;
}
