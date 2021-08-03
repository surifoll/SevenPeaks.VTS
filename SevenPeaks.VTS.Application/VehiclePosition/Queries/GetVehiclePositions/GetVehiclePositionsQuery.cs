using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SevenPeaks.VTS.Application.Helpers;
using SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicles;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Infrastructure.Interfaces;
using SevenPeaks.VTS.Persistence;

namespace SevenPeaks.VTS.Application.VehiclePosition.Queries.GetVehiclePositions
{
    public class GetVehiclePositionsQuery : IGetVehiclePositionsQuery
    {
        private readonly IDatabaseService _context;
        private readonly IUriService _uriService;

        public GetVehiclePositionsQuery(IDatabaseService context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }
        
        public async Task<MessageResponse<PagedResults<GetVehiclePositionsModel>>> Execute(VehiclePositionsQuery query)
        {
            var entities = _context.VehiclePositions.OrderByDescending(p=>p.UpdatedDate).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.PlateNumber))
                entities = entities.Where(vehicle => vehicle.Vehicle.PlateNumber == query.PlateNumber);
            if(query.DateFrom != null && query.DateTo != null){
                entities = entities.Where(x => x.UpdatedDate >= query.DateFrom && x.UpdatedDate <= query.DateTo);
            }
            if(query.GetLast is true)
                entities = entities.OrderByDescending(p=>p.UpdatedDate).Take(1);
            
            var result = await PagedResultHelper.CreatePagedResults<Domain.Entities.VehiclePosition, GetVehiclePositionsModel>(_uriService,
                entities, query.Page, query.PageSize, query.Route, query.OtherQueryStrings);
            return new MessageResponse<PagedResults<GetVehiclePositionsModel>>()
            {
                ResponseCode = 200,
                Result =  result
            };
        }
    }
}