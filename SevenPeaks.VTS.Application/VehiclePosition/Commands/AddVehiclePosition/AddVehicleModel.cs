namespace SevenPeaks.VTS.Application.VehiclePosition.Commands.AddVehiclePosition
{
    public class AddVehiclePositionModel
    {
        public string DeviceId { get; set; }
        public string UserId { get; set; }
        
        public int VehicleId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}