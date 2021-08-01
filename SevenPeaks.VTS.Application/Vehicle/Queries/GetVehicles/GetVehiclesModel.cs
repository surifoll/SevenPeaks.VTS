using System.Collections.Generic;
using Newtonsoft.Json;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicles
{   
    public class GetVehiclesModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlateNumber { get; set; }
        public string UserId { get; set; }
        public string DeviceId { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
    }
}