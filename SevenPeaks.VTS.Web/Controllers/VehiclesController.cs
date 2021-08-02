using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicles;
using SevenPeaks.VTS.Application.VehiclePosition.Queries.GetVehiclePositions;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Web.Models;

namespace SevenPeaks.VTS.Web.Controllers
{
    [Authorize]
    public class VehiclePositionsController : Controller
    {
        private readonly ILogger<VehiclePositionsController> _logger;
        private readonly IGetVehiclesQuery _query;
        private readonly IGetVehiclePositionsQuery _positionsQuery;
     
        public VehiclePositionsController(ILogger<VehiclePositionsController> logger, IGetVehiclesQuery query, IGetVehiclePositionsQuery positionsQuery)
        {
            _logger = logger;
            _query = query;
            _positionsQuery = positionsQuery;
        }
     
        public async Task<IActionResult> Index(string plateNumber = "", int page =1, int pageSize= 10)
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
        
        public async Task<IActionResult> Positions(string plateNumber = "",int page =1, int pageSize= 10)
        {
            var last = await _positionsQuery.Execute(new VehiclePositionsQuery()
            {
                Route = Request.Path.Value,
                Page = page,
                PageSize = pageSize,
                PlateNumber = plateNumber,
                GetLast = true
            });
            ViewBag.Last = last.Result.Results.FirstOrDefault();
            var model = await _positionsQuery.Execute(new VehiclePositionsQuery()
            {
                Route = Request.Path.Value,
                Page = page,
                PageSize = pageSize,
                PlateNumber = plateNumber
            });
            
            return base.View(model.Result);
        }
        
     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}