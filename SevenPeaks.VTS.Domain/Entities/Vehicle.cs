using System;
using System.Collections.Generic;

namespace SevenPeaks.VTS.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string DeviceCode { get; set; }
        public string Name { get; set; }
        public string PlateNumber { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }  
        public IEnumerable<VehiclePosition> VehiclePositions { get; set; }  
    }
}       