using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using OneForAll.EFCore;
using System.Reflection.Emit;

namespace UnitTest.Host
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options):base(options)
        {

        }
        public virtual DbSet<Order> Order { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
            });
        }
    }
}
