using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Application.VehiclePosition.Queries.GetVehiclePositions
{   
    public class GetVehiclePositionsModel
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime DateCreated { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
    public class VehiclePositionsQuery: QueryableResult
    {
        public string PlateNumber { get; set; }
        public bool? GetLast { get; set; }
    }
}