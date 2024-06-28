using System;
using System.Collections.Generic;

namespace app.Models;

public partial class Request
{
    public int Idrequest { get; set; }

    public string StudentId { get; set; } = null!;

    public int InstructorId { get; set; }

    public int StateId { get; set; }

    public virtual Instructor Instructor { get; set; } = null!;

    public virtual State State { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
