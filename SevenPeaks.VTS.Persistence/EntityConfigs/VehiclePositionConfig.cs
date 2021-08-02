using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SevenPeaks.VTS.Domain.Entities;

namespace SevenPeaks.VTS.Persistence.EntityConfigs
{
    
     public class VehiclePositionConfig: IEntityTypeConfiguration<Domain.Entities.VehiclePosition>
        {
             
            public void Configure(EntityTypeBuilder<Domain.Entities.VehiclePosition> builder)
            {
                builder.HasKey(p => p.Id);
                builder.HasOne(p => p.Vehicle)
                    .WithMany(p => p.VehiclePositions)
                    .HasForeignKey(o => o.VehicleId);
                
            }
        }
     public class VehicleConfig: IEntityTypeConfiguration<Domain.Entities.Vehicle>
     {
             
         public void Configure(EntityTypeBuilder<Domain.Entities.Vehicle> builder)
         {
             builder.HasKey(p => p.Id);
             builder.Property(p => p.DeviceCode).IsRequired();

         }
     }
}