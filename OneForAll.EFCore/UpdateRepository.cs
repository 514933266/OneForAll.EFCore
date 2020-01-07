using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OneForAll.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OneForAll.Core.ORM;
using OneForAll.Core.Utility;
using OneForAll.Core.Extension;
using EFCore.BulkExtensions;
using System.Threading.Tasks;

namespace OneForAll.EFCore
{
    partial class Repository<T>
    {
        public virtual async Task<int> UpdateAsync(T entity)
        {
            DbSet.Update(entity);
            return await SaveChangesAsync();
        }

        public virtual async Task<int> UpdateAsync(IEnumerable<T> entities)
        {
            DbSet.UpdateRange(entities);
            return await SaveChangesAsync();
        }

        public virtual async Task<int> BulkUpdateAsync(IList<T> entities)
        {
            Context.BulkUpdate(entities);
            return await SaveChangesAsync();
        }

        public virtual void Update(T entity, IUnitTransaction tran)
        {
            tran.Register(() =>
            {
                DbSet.Update(entity);
                return SaveChanges();
            }, Context);
        }
    }
}
