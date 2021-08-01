using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SevenPeaks.VTS.Application.Vehicle.Commands.AddVehicle;
using SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicles;
using SevenPeaks.VTS.Application.VehiclePosition.Commands.AddVehiclePosition;
using SevenPeaks.VTS.Application.VehiclePosition.Queries.GetVehiclePositions;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Infrastructure.Interfaces;

namespace SevenPeaks.VTS.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IGetVehiclesQuery _vehiclesQuery;
        private readonly IAddVehicleCommand _addVehicleCommand;
        private readonly IAddVehiclePositionCommand _addVehiclePositionCommand;
        private readonly IGetVehiclePositionsQuery _vehiclePositionsQuery;
        private readonly IStandardRabbitMq _rabbitMq;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IGetVehiclesQuery vehiclesQuery, IGetVehiclePositionsQuery vehiclePositionsQuery, IAddVehicleCommand addVehicleCommand, IAddVehiclePositionCommand addVehiclePositionCommand, IStandardRabbitMq rabbitMq, ILogger<VehiclesController> logger)
        {
            _vehiclesQuery = vehiclesQuery;
            _vehiclePositionsQuery = vehiclePositionsQuery;
            _addVehicleCommand = addVehicleCommand;
            _addVehiclePositionCommand = addVehiclePositionCommand;
            _rabbitMq = rabbitMq;
            _logger = logger;
        }
        [HttpGet("GetVehicles")]
        public async Task<MessageResponse<PagedResults<GetVehiclesModel>>> Vehicles(int page = 1, int pageSize = 10)
        {
            return  await _vehiclesQuery.Execute(new QueryableResult()
            {
                Page = page,
                PageSize = pageSize,
                Route = Request.Path.Value,
            });
        }
        
        [HttpGet("GetVehiclePositions")]
        public async Task<MessageResponse<PagedResults<GetVehiclePositionsModel>>> VehiclePositions(string plateNumber,int page = 1, int pageSize = 10)
        {
            return  await _vehiclePositionsQuery.Execute(new VehiclePositionsQuery()
            {
                 Page = page,
                PageSize = pageSize,
                Route = Request.Path.Value,
                PlateNumber = plateNumber,
            });
        }
        
        [HttpPost("AddVehicle")]
        public async Task<IActionResult> AddVehicle(AddVehicleModel command)
        {
            var result = await _addVehicleCommand.Execute(command);

            if (result.ResponseCode == 200)
                return Ok(result);
            return BadRequest(result);
        }
        
        [HttpPost("AddVehiclePosition")]
        public async Task<IActionResult> AddVehiclePosition(AddVehiclePositionModel command)
        {
             _rabbitMq.Publish(command);
             _logger.LogInformation("position added");
             return Ok();
        }
        
        
    }
}