using Microsoft.EntityFrameworkCore;
using ModelLayer;

namespace QuantityMeasurementWebApi.Data
{
    public class QuantityDbContext : DbContext
    {
        public QuantityDbContext(DbContextOptions<QuantityDbContext> options)
            : base(options)
        {
        }

        public DbSet<QuantityMeasurementEntity> Measurements { get; set; } = null!;
        public DbSet<UserEntity> Users { get; set; } = null!;
    }
}
