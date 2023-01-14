using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Actor
{
    public int IdActor { get; set; }

    public string Name { get; set; } = null!;

    public int? IdImage { get; set; }

    public virtual Image? IdImageNavigation { get; set; }

    public virtual ICollection<Role> Roles { get; } = new List<Role>();
}
