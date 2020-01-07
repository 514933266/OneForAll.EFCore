using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OneForAll.EFCore
{
    partial class Repository<T>
    {
        public virtual int StoreProcedure(string sql, params object[] parms)
        {
            return Context.Database.ExecuteSqlRaw(sql, parms);
        }

        public virtual IEnumerable<T> QueryStoreProcedure(string sql, params object[] parms)
        {
            return DbSet.FromSqlRaw(sql, parms).ToList();
        }
    }
}
