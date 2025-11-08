using Microsoft.EntityFrameworkCore;

namespace UnitTest.Host
{
    public class TestSlaveDbContext : TestDbContext
    {
        public TestSlaveDbContext(DbContextOptions<TestDbContext> options)
           : base(options)
        {

        }
    }
}
