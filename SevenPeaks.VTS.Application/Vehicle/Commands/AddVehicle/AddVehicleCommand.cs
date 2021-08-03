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
                
                var entity = new Domain.Entities.Vehicle()
                {
                    Name = command.Name,
                    CreatedDate = DateTime.Now,
                    PlateNumber = command.PlateNumber.Cleanup(),
                    UpdatedDate = DateTime.Now,
                    UserId = command.UserId,
                    DeviceCode =  Guid.NewGuid().ToString(),
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