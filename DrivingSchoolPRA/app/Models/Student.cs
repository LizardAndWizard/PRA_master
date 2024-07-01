using System;
using System.Collections.Generic;

namespace app.Models;

public partial class Student
{
    public string Oib { get; set; } = null!;

    public int PersonId { get; set; }

    public int? InstructorId { get; set; }

    public int? VehicleId { get; set; }

    public int? HoursDriven { get; set; }

    public virtual Instructor? Instructor { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; } = new List<Request>();

    public virtual ICollection<Review> Reviews { get; } = new List<Review>();

    public virtual ICollection<Rezervation> Rezervations { get; } = new List<Rezervation>();

    public virtual Vehicle? Vehicle { get; set; }
}
