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
            var entity = _context.Vehicles.First(vehicle => vehicle.Id == command.Id);
            if (entity == null)
            {
                return new MessageResponse<int>("Vehicle already exists")
                {
                    ResponseCode = 400
                };
            }
            if(!string.IsNullOrWhiteSpace(command.Name))
                entity.Name = command.Name;
             
            
            entity.UpdatedDate = DateTime.Now;
            entity.UserId = command.UserId;
            if(!string.IsNullOrWhiteSpace(command.CustomFields))
                entity.CustomFields = command.CustomFields;
            
             var result = await _context.SaveAsync();
             return new MessageResponse<int>("Vehicle updated")
             {
                 ResponseCode = 200,
                 Result = result
             };
        }
    }
}