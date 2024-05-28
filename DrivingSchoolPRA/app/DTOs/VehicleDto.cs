using app.Models;

namespace app.DTOs
{
    public class VehicleDto
    {
        public int Idvehicle { get; set; }

        public byte[]? Picture { get; set; }

        public virtual string Category { get; set; } = null!;

        public virtual string Colour { get; set; } = null!;

        public virtual string Model { get; set; } = null!;

        public virtual string Brand { get; set; } = null!;
    }
}
