using System.Collections.Generic;
using Newtonsoft.Json;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Application.Vehicle.Commands.AddVehicle
{
    public class AddVehicleModel
    {
        public string Name { get; set; }
        public string PlateNumber { get; set; }
        public string CustomFields => JsonConvert.SerializeObject(AdditionalFields);
        public List<CustomField> AdditionalFields { get; set; }   
        public string UserId { get; set; }
    }
}