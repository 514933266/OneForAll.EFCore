using Microsoft.EntityFrameworkCore;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneForAll.EFCore
{
    partial class Repository<T>
    {
        public virtual async Task<T> FindAsync(params object[] primaryKeys)
        {
            return await DbSet.FindAsync(primaryKeys);
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet
                .AsNoTracking()
                .Where(predicate)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> GetListAsync()
        {
            return await DbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        }
    }
}
