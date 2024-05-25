using System;
using System.Collections.Generic;

namespace app.Models;

public partial class Review
{
    public int Idreview { get; set; }

    public string StudentId { get; set; } = null!;

    public int InstructorId { get; set; }

    public int Grade { get; set; }

    public string? Comment { get; set; }

    public virtual Instructor Instructor { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
