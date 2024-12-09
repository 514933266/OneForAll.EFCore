using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;

namespace UnitTest.Host
{
    public class TestSlaveDbContext : DbContext
    {
        public TestSlaveDbContext(DbContextOptions<TestSlaveDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Order> Order { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order2025");
            });
        }
    }
}
