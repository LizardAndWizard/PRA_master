using System;
using System.Collections.Generic;

namespace app.Models;

public partial class Vehicle
{
    public int Idvehicle { get; set; }

    public int ColourId { get; set; }

    public int CategoryId { get; set; }

    public int ModelId { get; set; }

    public int InstructorId { get; set; }

    public byte[]? Picture { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Colour Colour { get; set; } = null!;

    public virtual Instructor Instructor { get; set; } = null!;

    public virtual Model Model { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; } = new List<Request>();

    public virtual ICollection<Student> Students { get; } = new List<Student>();
}
