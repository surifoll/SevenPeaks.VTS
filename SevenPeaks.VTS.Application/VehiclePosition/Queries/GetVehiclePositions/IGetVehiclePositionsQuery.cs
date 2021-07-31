using System.Threading.Tasks;
using SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicles;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Application.VehiclePosition.Queries.GetVehiclePositions
{
    public interface IGetVehiclePositionsQuery
    {
        Task<MessageResponse<PagedResults<GetVehiclePositionsModel>>> Execute(VehiclePositionsQuery query);
    }
}