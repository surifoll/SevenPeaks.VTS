using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SevenPeaks.VTS.Application.Helpers;
using SevenPeaks.VTS.Common.Models;
using SevenPeaks.VTS.Infrastructure.Interfaces;
using SevenPeaks.VTS.Persistence;

namespace SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicles
{
    public class GetVehiclesQuery : IGetVehiclesQuery
    {
        private readonly IDatabaseService _context;
        private readonly IUriService _uriService;

        public GetVehiclesQuery(IDatabaseService context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }
        
        public async Task<MessageResponse<PagedResults<GetVehiclesModel>>> Execute(VehiclesQuery query)
        {
            var entities = _context.Vehicles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.PlateNumber))
                entities = entities.Where(p => p.PlateNumber.Contains(query.PlateNumber));
            var result = await PagedResultHelper.CreatePagedResults<Domain.Entities.Vehicle, GetVehiclesModel>(_uriService,
                entities, query.Page, query.PageSize, query.Route, query.OtherQueryStrings);
            return new MessageResponse<PagedResults<GetVehiclesModel>>()
            {
                ResponseCode = 200,
                Result =  result
            };
        }
        public async Task<bool> Execute(int vehicleId, string deviceId)
        {   
            var anyAsync = await  _context.Vehicles.AnyAsync(vehicle => vehicle.DeviceCode == deviceId && vehicle.Id == vehicleId);
            return anyAsync;
        }
    }
}