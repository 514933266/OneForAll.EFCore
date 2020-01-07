using EFCore.BulkExtensions;
using OneForAll.Core.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneForAll.EFCore
{
    partial class Repository<T>
    {
        public virtual async Task<int> DeleteAsync(T entity)
        {
            DbSet.Remove(entity);
            return await SaveChangesAsync();
        }

        public virtual async Task<int> DeleteRangeAsync(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
            return await SaveChangesAsync();
        }

        public virtual async Task<int> BulkDeleteAsync(IList<T> entities)
        {
            Context.BulkDelete(entities);
            return await SaveChangesAsync();
        }

        public virtual void Delete(T entity, IUnitTransaction tran)
        {
            tran.Register(() =>
            {
                DbSet.Remove(entity);
                return SaveChanges();
            }, Context);
        }

        public virtual void Delete(Expression<Func<T, bool>> predicate, IUnitTransaction tran)
        {
            tran.Register(() =>
            {
                var entities = DbSet
               .Where(predicate)
               .ToList();

                DbSet.RemoveRange(entities);
                return SaveChanges();
            }, Context);
        }

        public virtual void BulkDelete(Expression<Func<T, bool>> predicate, IUnitTransaction tran)
        {
            tran.Register(() =>
            {
                var entities = DbSet
               .Where(predicate)
               .ToList();

                Context.BulkDelete(entities);
                return SaveChanges();
            }, Context);
        }
    }
}
