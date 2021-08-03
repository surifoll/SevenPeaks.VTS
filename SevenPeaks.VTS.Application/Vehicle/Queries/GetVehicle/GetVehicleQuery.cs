using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SevenPeaks.VTS.Persistence;

namespace SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicle
{
    public class GetVehicleQuery : IGetVehicleQuery
    {
        private readonly IDatabaseService _context;

        public GetVehicleQuery(IDatabaseService context)
        {
            _context = context;
        }
        
        public async Task<bool> Execute(int vehicleId, string deviceId)
        {   
            var anyAsync = await  _context.Vehicles.AnyAsync(vehicle => vehicle.DeviceCode == deviceId && vehicle.Id == vehicleId);
            return anyAsync;
        }

        public async Task<GetVehicleModel> Execute(int id)
        {
            var data =  await  _context.Vehicles.FirstOrDefaultAsync(vehicle => vehicle.Id == id);

            if (data != null)
                return new GetVehicleModel
                {
                    Model = data.Model,
                    Year = data.Year,
                    DeviceId = data.DeviceCode,
                    DateUpdated = data.UpdatedDate,
                    UserId = data.UserId
                };
            return null;
        }
    }
}