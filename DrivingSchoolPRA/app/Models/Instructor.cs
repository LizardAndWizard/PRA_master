using System;
using System.Collections.Generic;

namespace app.Models;

public partial class Instructor
{
    public int Idinstructor { get; set; }

    public int PersonId { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; } = new List<Review>();

    public virtual ICollection<Rezervation> Rezervations { get; } = new List<Rezervation>();

    public virtual ICollection<Vehicle> Vehicles { get; } = new List<Vehicle>();
}
