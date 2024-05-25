using System;
using System.Collections.Generic;

namespace app.Models;

public partial class Person
{
    public int Idperson { get; set; }

    public string FirstName { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Instructor> Instructors { get; } = new List<Instructor>();

    public virtual Student? Student { get; set; }
}
