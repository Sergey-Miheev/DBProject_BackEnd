using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Booking
{
    public int IdBooking { get; set; }

    public int IdSession { get; set; }

    public int IdPlace { get; set; }

    public int IdAccount { get; set; }

    public string BookingCode { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public virtual Account IdAccountNavigation { get; set; } = null!;

    public virtual Place IdPlaceNavigation { get; set; } = null!;

    public virtual Session IdSessionNavigation { get; set; } = null!;
}
