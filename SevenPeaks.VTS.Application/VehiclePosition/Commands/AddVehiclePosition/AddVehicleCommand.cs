using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SevenPeaks.VTS.Application.Vehicle.Commands.AddVehicle;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Persistence;

namespace SevenPeaks.VTS.Application.VehiclePosition.Commands.AddVehiclePosition
{
    public class AddVehiclePositionCommand : IAddVehiclePositionCommand
    {
        private readonly IDatabaseService _context;
        private readonly ILogger<AddVehiclePositionCommand> _logger;
        public AddVehiclePositionCommand(IDatabaseService context, ILogger<AddVehiclePositionCommand> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<MessageResponse<int>> Execute(AddVehiclePositionModel command)
        {
            if (!_context.Vehicles.Any(vehicle => vehicle.Id == command.VehicleId && vehicle.DeviceCode == command.DeviceId && vehicle.IsActive))
            {
                _logger.LogInformation("Vehicle not found");
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