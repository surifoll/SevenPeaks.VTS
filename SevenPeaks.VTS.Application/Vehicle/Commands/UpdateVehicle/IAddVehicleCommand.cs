using System.Threading.Tasks;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Application.Vehicle.Commands.UpdateVehicle
{
    public interface IUpdateVehicleCommand
    {
        Task<MessageResponse<int>> Execute(UpdateVehicleModel command);
    }
}