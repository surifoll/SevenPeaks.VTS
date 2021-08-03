using System.Threading.Tasks;
using SevenPeaks.VTS.Common.Models;

namespace SevenPeaks.VTS.Application.Vehicle.Commands.AddVehicle
{
    public interface IAddVehicleCommand
    {
        Task<MessageResponse<string>> Execute(AddVehicleModel command);
    }
}