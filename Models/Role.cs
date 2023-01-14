using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Role
{
    public int IdRole { get; set; }

    public int IdActor { get; set; }

    public int IdFilm { get; set; }

    public string NamePersonage { get; set; } = null!;

    public virtual Actor IdActorNavigation { get; set; } = null!;

    public virtual Film IdFilmNavigation { get; set; } = null!;
}
