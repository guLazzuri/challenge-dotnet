
using challenge.Domain.Entity;
using challenge.Infra.Data.Mappings;
using challenge.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace challenge.Infrastructure.Context
{
    public class ChallengeContext(DbContextOptions<ChallengeContext> options) : DbContext(options)
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<MaintenanceHistory> MaintenanceHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new VehicleMapping());
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new MaintenanceHistoryMapping());

        }

    }
}
