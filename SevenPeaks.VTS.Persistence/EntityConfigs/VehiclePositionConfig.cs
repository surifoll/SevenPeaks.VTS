using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SevenPeaks.VTS.Persistence.EntityConfigs
{
    
     public class VehiclePositionConfig: IEntityTypeConfiguration<Domain.Entities.VehiclePosition>
        {
             
            public void Configure(EntityTypeBuilder<Domain.Entities.VehiclePosition> builder)
            {
                builder.HasKey(p => p.Id);
               // builder.HasOne(p=>p.Vehicle).WithOne(p=>p.VehiclePositions).HasForeignKey<Vehicle>(p => p.OtpId);
            }
        }
}