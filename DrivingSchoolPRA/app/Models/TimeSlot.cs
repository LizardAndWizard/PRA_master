using System;
using System.Collections.Generic;

namespace app.Models;

public partial class TimeSlot
{
    public int IdtimeSlot { get; set; }

    public int RezervationId { get; set; }

    public bool? Done { get; set; }

    public virtual Rezervation Rezervation { get; set; } = null!;
}
