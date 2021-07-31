using System;
using System.Linq;
using System.Threading.Tasks;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Domain.Entities;
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
            try
            {

                if (_context.Vehicles.Any(vehicle => vehicle.PlateNumber == command.PlateNumber))
                {
                    return new MessageResponse<int>("Vehicle already exists")
                    {
                        ResponseCode = 400
                    };
                }
                
                if (_context.Vehicles.Any(vehicle => vehicle.DeviceId == command.DeviceId && vehicle.IsActive))
                {
                    return new MessageResponse<int>("Device is inuse by a vehicle")
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
                    DeviceId =  command.DeviceId,
                    Model = command.Model,
                    Year = command.Year,
                    IsActive = true,
                };

                _context.Vehicles.Add(entity);
                var result = await _context.SaveAsync();
                return new MessageResponse<int>("Vehicle create")
                {
                    ResponseCode = 200,
                    Result = result
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}