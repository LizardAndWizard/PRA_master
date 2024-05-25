using System;
using System.Collections.Generic;

namespace app.Models;

public partial class Student
{
    public string Oib { get; set; } = null!;

    public int PersonId { get; set; }

    public int? HoursDriven { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; } = new List<Review>();

    public virtual ICollection<Rezervation> Rezervations { get; } = new List<Rezervation>();
}
