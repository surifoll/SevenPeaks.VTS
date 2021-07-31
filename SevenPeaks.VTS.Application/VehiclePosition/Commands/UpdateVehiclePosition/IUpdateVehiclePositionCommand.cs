using System.Threading.Tasks;
using SevenPeaks.VTS.Application.Vehicle.Commands.UpdateVehicle;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Application.VehiclePosition.Commands.UpdateVehiclePosition
{
    public interface IUpdateVehiclePositionCommand
    {
        Task<MessageResponse<int>> Execute(UpdateVehiclePositionModel command);
    }
}