using System;
using System.Collections.Generic;

namespace app.Models;

public partial class Rezervation
{
    public int Idrezervation { get; set; }

    public string StudentId { get; set; } = null!;

    public int InstructorId { get; set; }

    public int StateId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual Instructor Instructor { get; set; } = null!;

    public virtual State State { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual ICollection<TimeSlot> TimeSlots { get; } = new List<TimeSlot>();
}
