using System;
using System.Linq;
using System.Threading.Tasks;
using SevenPeaks.VTS.Application.Vehicle.Commands.UpdateVehicle;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Persistence;

namespace SevenPeaks.VTS.Application.VehiclePosition.Commands.UpdateVehiclePosition
{
    public class UpdateVehiclePositionCommand : IUpdateVehiclePositionCommand
    {
        private readonly IDatabaseService _context;

        public UpdateVehiclePositionCommand(IDatabaseService context)
        {
            _context = context;
        }
        
        public async Task<MessageResponse<int>> Execute(UpdateVehiclePositionModel command)
        {
            var entity = _context.VehiclePositions
                .FirstOrDefault(vehicle => vehicle.Id == command.Id && vehicle.VehicleId == command.VehicleId);
           
            if (entity == null)
            {
                return new MessageResponse<int>("Vehicle not found!")
                {
                    ResponseCode = 404
                };
            }

            entity.Longitude = command.Longitude;
            entity.Latitude = command.Latitude;
            entity.UpdatedDate = DateTime.Now;
            entity.UserId = command.UserId;
            
            
            var result = await _context.SaveAsync();
            return new MessageResponse<int>("Vehicle updated")
            {
                ResponseCode = 200,
                Result = result
            };
        }
    }
}