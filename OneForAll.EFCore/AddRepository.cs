using OneForAll.Core.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFCore.BulkExtensions;

namespace OneForAll.EFCore
{
    partial class Repository<T>
    {
        public virtual async Task<int> AddAsync(T entity)
        {
            DbSet.Add(entity);
            return await SaveChangesAsync();
        }
        public virtual async Task<int> AddRangeAsync(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);
            return await SaveChangesAsync();
        }

        public virtual async Task<int> BulkInsertAsync(IList<T> entities)
        {
            Context.BulkInsert(entities);
            return await SaveChangesAsync();
        }

        public virtual void Add(T entity, IUnitTransaction tran)
        {
            tran.Register(() =>
            {
                DbSet.Add(entity);
                return SaveChanges();
            }, Context);
        }

        public virtual void AddList(IEnumerable<T> entities, IUnitTransaction tran)
        {
            tran.Register(() =>
            {
                DbSet.AddRange(entities);
                return SaveChanges();
            }, Context);
        }

        public virtual void AddIfNotExists(T entity, Expression<Func<T, bool>> predicate, IUnitTransaction tran)
        {
            tran.Register(() =>
            {
                var data = DbSet.Where(predicate).FirstOrDefault();
                if (data == null)
                {
                    DbSet.Add(entity);
                }
                return SaveChanges();
            }, Context);
        }
    }
}
