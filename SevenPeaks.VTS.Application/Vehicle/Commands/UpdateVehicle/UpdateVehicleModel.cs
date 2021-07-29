using System.Collections.Generic;
using Newtonsoft.Json;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Application.Vehicle.Commands.UpdateVehicle
{   
    public class UpdateVehicleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlateNumber { get; set; }
        public string CustomFields => AdditionalFields.Count > 0 ? JsonConvert.SerializeObject(AdditionalFields) : null;
        public List<CustomField> AdditionalFields { get; set; }   
        public string UserId { get; set; }
    }
}