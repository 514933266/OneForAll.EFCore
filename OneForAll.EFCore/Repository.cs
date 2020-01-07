using Microsoft.EntityFrameworkCore;
using OneForAll.Core.ORM;
using System.Threading.Tasks;

namespace OneForAll.EFCore
{
    public partial class Repository<T> : IEFCoreRepository<T> where T : class
    {

        public virtual DbContext Context { get; set; }
        public virtual DbSet<T> DbSet { get; set; }

        public Repository(DbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public virtual int Execute(string sql, params object[] parms)
        {
            return Context.Database.ExecuteSqlRaw(sql, parms);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
