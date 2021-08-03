using System;
using System.Linq;
using System.Threading.Tasks;
using SevenPeaks.VTS.Application.Vehicle.Commands.AddVehicle;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Persistence;

namespace SevenPeaks.VTS.Application.Vehicle.Commands.UpdateVehicle
{
    public class UpdateVehicleCommand : IUpdateVehicleCommand
    {
        private readonly IDatabaseService _context;

        public UpdateVehicleCommand(IDatabaseService context)
        {
            _context = context;
        }
        
        public async Task<MessageResponse<int>> Execute(UpdateVehicleModel command)
        {
            var entity = _context.Vehicles
                .First(vehicle => vehicle.Id == command.Id && vehicle.DeviceCode == command.DeviceId);
           
            if (entity == null)
            {
                return new MessageResponse<int>("Vehicle not found!")
                {
                    ResponseCode = 404
                };
            }
            if(!string.IsNullOrWhiteSpace(command.Model))
                entity.Model = command.Model;
            if(command.Year> 1990)
                entity.Year = command.Year;
            if(command.IsActive.HasValue)
                entity.IsActive = command.IsActive.Value;
           
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