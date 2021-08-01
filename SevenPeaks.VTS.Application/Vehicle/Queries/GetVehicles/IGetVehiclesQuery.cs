using System.Collections.Generic;
using System.Threading.Tasks;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicles
{
    public interface IGetVehiclesQuery
    {
        Task<MessageResponse<PagedResults<GetVehiclesModel>>> Execute(QueryableResult query);
        Task<bool> Execute(int vehicleId, string deviceId);
    }
}