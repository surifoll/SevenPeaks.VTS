namespace SevenPeaks.VTS.Common.Models
{
    public class RabbitMqSettings
    {
        public string ConnectionString { get; set; }
        public string VehiclePositionQueue { get; set; }
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}