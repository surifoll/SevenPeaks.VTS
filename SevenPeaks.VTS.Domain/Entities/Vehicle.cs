using System;
namespace SevenPeaks.VTS.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PlateNumber { get; set; }
        public string CustomFields { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UserId { get; set; }
    }
}       
