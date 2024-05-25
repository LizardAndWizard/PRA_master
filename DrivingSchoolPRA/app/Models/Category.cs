using System;
using System.Collections.Generic;

namespace app.Models;

public partial class Category
{
    public int Idcategory { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Vehicle> Vehicles { get; } = new List<Vehicle>();
}
