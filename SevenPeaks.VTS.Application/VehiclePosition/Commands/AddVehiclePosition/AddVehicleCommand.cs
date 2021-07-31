using System;
using System.Linq;
using System.Threading.Tasks;
using SevenPeaks.VTS.Application.Vehicle.Commands.AddVehicle;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Persistence;

namespace SevenPeaks.VTS.Application.VehiclePosition.Commands.AddVehiclePosition
{
    public class AddVehiclePositionCommand : IAddVehiclePositionCommand
    {
        private readonly IDatabaseService _context;

        public AddVehiclePositionCommand(IDatabaseService context)
        {
            _context = context;
        }


        public async Task<MessageResponse<int>> Execute(AddVehiclePositionModel command)
        {
            if (!_context.Vehicles.Any(vehicle => vehicle.Id == command.VehicleId && vehicle.DeviceId == command.DeviceId && vehicle.IsActive))
            {
                return new MessageResponse<int>("Vehicle not found")
                {
                    ResponseCode = 404
                };
            }

            var entity = new Domain.Entities.VehiclePosition()
            {
                VehicleId = command.VehicleId,
                CreatedDate = DateTime.Now,
                Latitude = command.Latitude,
                Longitude = command.Longitude,
                UpdatedDate = DateTime.Now,
                UserId = command.UserId,
            };

            _context.VehiclePositions.Add(entity);
             var result = await _context.SaveAsync();
             return new MessageResponse<int>("Vehicle create")
             {
                 ResponseCode = 200,
                 Result = result
             };
        }
    }
}