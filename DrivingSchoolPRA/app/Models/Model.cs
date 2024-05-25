using System;
using System.Collections.Generic;

namespace app.Models;

public partial class Model
{
    public int Idmodel { get; set; }

    public string Name { get; set; } = null!;

    public int BrandId { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<Vehicle> Vehicles { get; } = new List<Vehicle>();
}
