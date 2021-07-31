
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SevenPeaks.VTS.Domain.Entities;
using SevenPeaks.VTS.Infrastructure.Interfaces;

namespace SevenPeaks.VTS.Persistence
{

    public interface IDatabaseService
    {
        public DbSet<Audit> AuditLogs { get; set; }
        DbSet<Vehicle> Vehicles { get; set; }
        DbSet<VehiclePosition> VehiclePositions { get; set; }
        Task<int> SaveAsync();
    }

    
    public class DatabaseService : AuditableIdentityContext, IDatabaseService
    {
        readonly IAuthenticatedUser _user;
        public DatabaseService(DbContextOptions<DatabaseService> options, IAuthenticatedUser user) : base(options)
        {
            Database.EnsureCreated();
            _user = user;
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehiclePosition> VehiclePositions { get; set; }

        public Task<int> SaveAsync()
        {
            return base.SaveChangesAsync(_user.UserId);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
 
