using FluentValidation;
using SevenPeaks.VTS.Application.Vehicle.Queries.GetVehicles;
using SevenPeaks.VTS.Application.VehiclePosition.Commands.AddVehiclePosition;

namespace SevenPeaks.VTS.Web.Validations
{
    public class VehiclePositionValidator : AbstractValidator<AddVehiclePositionModel>
    {
        public VehiclePositionValidator(IGetVehiclesQuery userDetailQuery)
        {
            // IGetVehiclesQuery _userDetailQuery = new GetVehiclesQuery(new DatabaseService(new DbContextOptions<DatabaseService>()));
            RuleFor(x => x.Latitude).GreaterThan(0);
            RuleFor(x => x.Longitude).GreaterThan(0);
            RuleFor(x => x.DeviceId).Must((model, confirmPassword) => userDetailQuery
                    .Execute(model.VehicleId, model.DeviceId).GetAwaiter().GetResult())
                .WithMessage("Either the device Id or the Vehicle Id is incorrect.");
        }
    }
}