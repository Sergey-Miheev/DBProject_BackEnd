using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Account
{
    public int IdAccount { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateOnly DateOfBirthday { get; set; }

    public short Role { get; set; }

    public int? IdImage { get; set; }

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    public virtual Image? IdImageNavigation { get; set; }

}