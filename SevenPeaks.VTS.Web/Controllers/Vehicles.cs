using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SevenPeaks.VTS.Application.Vehicle.Commands.AddVehicle;
using SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicles;
using SevenPeaks.VTS.Application.VehiclePosition.Commands.AddVehiclePosition;
using SevenPeaks.VTS.Application.VehiclePosition.Queries.GetVehiclePositions;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Vehicles : ControllerBase
    {
        private readonly IGetVehiclesQuery _vehiclesQuery;
        private readonly IAddVehicleCommand _addVehicleCommand;
        private readonly IAddVehiclePositionCommand _addVehiclePositionCommand;
        private readonly IGetVehiclePositionsQuery _vehiclePositionsQuery;

        public Vehicles(IGetVehiclesQuery vehiclesQuery, IGetVehiclePositionsQuery vehiclePositionsQuery, IAddVehicleCommand addVehicleCommand, IAddVehiclePositionCommand addVehiclePositionCommand)
        {
            _vehiclesQuery = vehiclesQuery;
            _vehiclePositionsQuery = vehiclePositionsQuery;
            _addVehicleCommand = addVehicleCommand;
            _addVehiclePositionCommand = addVehiclePositionCommand;
        }
        [HttpGet("GetVehicles")]
        public async Task<MessageResponse<PagedResults<GetVehiclesModel>>> GetVehicles()
        {
            return  await _vehiclesQuery.Execute(new QueryableResult()
            {
                Route = Request.Path.Value,
            });
        }
        
        [HttpGet("GetVehiclePositions")]
        public async Task<MessageResponse<PagedResults<GetVehiclePositionsModel>>> GetVehiclePositions(string plateNumber)
        {
            return  await _vehiclePositionsQuery.Execute(new VehiclePositionsQuery()
            {
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
            var result = await _addVehiclePositionCommand.Execute(command);

            if (result.ResponseCode == 200)
                return Ok(result);
            return BadRequest(result);
        }
        
        
    }
}