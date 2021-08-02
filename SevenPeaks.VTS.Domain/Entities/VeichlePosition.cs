using System;
namespace SevenPeaks.VTS.Domain.Entities
{
    public class VehiclePosition
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UserId { get; set; }  
        public Vehicle Vehicle { get; set; }
    }
}
