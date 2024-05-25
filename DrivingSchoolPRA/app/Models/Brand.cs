using System;
using System.Collections.Generic;

namespace app.Models;

public partial class Brand
{
    public int Idbrand { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Model> Models { get; } = new List<Model>();
}
