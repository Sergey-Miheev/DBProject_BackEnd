using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Film
{
    public int IdFilm { get; set; }

    public string Duration { get; set; } = null!;

    public string Name { get; set; } = null!;

    public short AgeRating { get; set; }

    public string Description { get; set; } = null!;

    public int? IdImage { get; set; }

    public virtual ICollection<Role> Roles { get; } = new List<Role>();

    public virtual ICollection<Session> Sessions { get; } = new List<Session>();
}
