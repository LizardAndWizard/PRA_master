using System;
using System.Collections.Generic;

namespace app.Models;

public partial class State
{
    public int Idstate { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Rezervation> Rezervations { get; } = new List<Rezervation>();
}
