using System.Collections.Generic;
using Newtonsoft.Json;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Application.VehiclePosition.Commands.UpdateVehiclePosition
{   
    public class UpdateVehiclePositionModel
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string UserId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}