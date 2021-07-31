using System.Threading.Tasks;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Application.VehiclePosition.Commands.AddVehiclePosition
{
    public interface IAddVehiclePositionCommand
    {
        Task<MessageResponse<int>> Execute(AddVehiclePositionModel command);
    }
}