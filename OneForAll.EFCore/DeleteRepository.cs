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
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> DeleteAsync(T entity)
        {
            DbSet.Remove(entity);
            return await SaveChangesAsync();
        }

        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> DeleteRangeAsync(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
            return await SaveChangesAsync();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> BulkDeleteAsync(IList<T> entities)
        {
            Context.BulkDelete(entities);
            return await SaveChangesAsync();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">事务</param>
        public virtual void Delete(T entity, IUnitTransaction tran)
        {
            tran.Register(() =>
            {
                DbSet.Remove(entity);
                return SaveChanges();
            }, Context);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="tran">事务</param>
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

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="tran">事务</param>
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
