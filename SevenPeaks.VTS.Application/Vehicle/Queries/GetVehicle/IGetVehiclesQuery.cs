using System.Collections.Generic;
using System.Threading.Tasks;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicle
{
    public interface IGetVehicleQuery
    {
        Task<GetVehicleModel> Execute(int id);
    }
}