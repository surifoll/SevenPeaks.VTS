using System;
using System.Linq;
using System.Threading.Tasks;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Persistence;

namespace SevenPeaks.VTS.Application.Vehicle.Commands.AddVehicle
{
    public class AddVehicleCommand : IAddVehicleCommand
    {
        private readonly IDatabaseService _context;

        public AddVehicleCommand(IDatabaseService context)
        {
            _context = context;
        }


        public async Task<MessageResponse<int>> Execute(AddVehicleModel command)
        {
            if (_context.Vehicles.Any(vehicle => vehicle.Name == command.Name && vehicle.PlateNumber == command.PlateNumber))
            {
                return new MessageResponse<int>("Vehicle already exists")
                {
                    ResponseCode = 400
                };
            }
            var entity = new Domain.Entities.Vehicle()
            {
                Name = command.Name,
                CreatedDate = DateTime.Now,
                PlateNumber = command.PlateNumber,
                UpdatedDate = DateTime.Now,
                UserId = command.UserId,
                CustomFields = command.CustomFields
            };

            _context.Vehicles.Add(entity);
             var result = await _context.SaveAsync();
             return new MessageResponse<int>("Vehicle create")
             {
                 ResponseCode = 200,
                 Result = result
             };
        }
    }
}