using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SevenPeaks.VTS.Common.ExtensionMethods;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Persistence;

namespace SevenPeaks.VTS.Application.Vehicle.Commands.AddVehicle
{
    public class AddVehicleCommand : IAddVehicleCommand
    {
        private readonly IDatabaseService _context;
        private readonly ILogger<AddVehicleCommand> _logger;
        public AddVehicleCommand(IDatabaseService context, ILogger<AddVehicleCommand> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<MessageResponse<int>> Execute(AddVehicleModel command)
        {
            try
            {

                if (_context.Vehicles.Any(vehicle => vehicle.PlateNumber == command.PlateNumber))
                {
                    _logger.LogInformation($"Vehicle with the plate number already exists {command.PlateNumber}");
                    return new MessageResponse<int>("Vehicle with the plate number already exists")
                    {
                        ResponseCode = 400
                    };
                }
                if(!string.IsNullOrWhiteSpace(command.DeviceId))
                    if (_context.Vehicles.Any(vehicle => vehicle.DeviceCode == command.DeviceId && vehicle.IsActive))
                    {
                        _logger.LogInformation($"Device Id is inuse by a vehicle{command.PlateNumber}");
                        return new MessageResponse<int>("Device Id is inuse by a vehicle")
                        {
                            ResponseCode = 400
                        };
                    }
                
                var entity = new Domain.Entities.Vehicle()
                {
                    Name = command.Name,
                    CreatedDate = DateTime.Now,
                    PlateNumber = command.PlateNumber.Cleanup(),
                    UpdatedDate = DateTime.Now,
                    UserId = command.UserId,
                    DeviceCode =  string.IsNullOrWhiteSpace(command.DeviceId)? Guid.NewGuid().ToString(): command.DeviceId,
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