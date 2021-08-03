using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SevenPeaks.VTS.Application.Vehicle.Commands.AddVehicle;
using SevenPeaks.VTS.Application.Vehicle.Commands.UpdateVehicle;
using SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicle;
using SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicles;
using SevenPeaks.VTS.Application.VehiclePosition.Queries.GetVehiclePositions;
using SevenPeaks.VTS.Common.ExtensionMethods;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Web.Models;

namespace SevenPeaks.VTS.Web.Controllers
{
    [Authorize]
    public class VehiclePositionsController : Controller
    {
        private readonly ILogger<VehiclePositionsController> _logger;
        private readonly IGetVehiclesQuery _query;
        private readonly IGetVehicleQuery _vehicleQuery;
        private readonly IGetVehiclePositionsQuery _positionsQuery;
        private readonly IUpdateVehicleCommand _updateVehicleCommand;
        private readonly IAddVehicleCommand _addVehicleCommand;

        public VehiclePositionsController(ILogger<VehiclePositionsController> logger, IGetVehiclesQuery query, IGetVehiclePositionsQuery positionsQuery, IGetVehicleQuery vehicleQuery, IUpdateVehicleCommand updateVehicleCommand, IAddVehicleCommand addVehicleCommand)
        {
            _logger = logger;
            _query = query;
            _positionsQuery = positionsQuery;
            _vehicleQuery = vehicleQuery;
            _updateVehicleCommand = updateVehicleCommand;
            _addVehicleCommand = addVehicleCommand;
        }

        public async Task<IActionResult> Index(string plateNumber = "", int page = 1, int pageSize = 10)
        {
            var model = await _query.Execute(new VehiclesQuery
            {
                Route = Request.Path.Value,
                Page = page,
                PageSize = pageSize,
                PlateNumber = plateNumber
            });

            return base.View(model.Result);
        }

        public async Task<IActionResult> Positions(DateTime? dateFrom, DateTime? dateTo, string plateNumber = "", int pageNumber = 1, int pageSize = 10)
        {
            // To select the last/current record

            var keyValuePairs = HttpContext.Request.Query.ToList();
            var output = keyValuePairs.GetQueryString();
            var last = await _positionsQuery.Execute(new VehiclePositionsQuery()
            {
                Route = Request.Path.Value,
                Page = 1,
                PageSize = 1,
                PlateNumber = plateNumber,
                GetLast = true
            });
            ViewBag.Last = last.Result?.Results?.FirstOrDefault();


            var model = await _positionsQuery.Execute(new VehiclePositionsQuery()
            {
                Route = Request.Path.Value,
                Page = pageNumber,
                PageSize = pageSize,
                PlateNumber = plateNumber,
                DateFrom = dateFrom,
                DateTo = dateTo
            });
            ViewBag.QueryStrings = output;
            return base.View(model.Result);
        }

        

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddVehicleModel command)
        {
            command.UserId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _addVehicleCommand.Execute(command);

            if (result.ResponseCode != 200)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            ViewBag.Success = "Successfully updated";
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _vehicleQuery.Execute((int)id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Model, DeviceId,Year,Id")] UpdateVehicleModel data)
        {

            var vehicle = await _vehicleQuery.Execute((int)id);
            if (id != data.Id)
            {
                ViewBag.Error = "Not found";
                return View(vehicle);
            }

            data.UserId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var updated = _updateVehicleCommand.Execute(data);
            ViewBag.Success = "Successfully updated";
            return View(vehicle);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}