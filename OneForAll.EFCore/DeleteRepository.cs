using EFCore.BulkExtensions;
using OneForAll.Core.ORM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneForAll.EFCore
{
    public partial class Repository<T>
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        public virtual int Delete(T entity)
        {
            DbSet.Remove(entity);
            return SaveChanges();
        }

        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        public virtual int DeleteRange(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
            return SaveChanges();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        public virtual int BulkDelete(IList<T> entities)
        {
            Context.BulkDelete(entities);
            return SaveChanges();
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
        /// <param name="entities">实体</param>
        /// <param name="tran">事务</param>
        public virtual void DeleteRange(IEnumerable<T> entities, IUnitTransaction tran)
        {
            tran.Register(() =>
            {
                DbSet.RemoveRange(entities);
                return SaveChanges();
            }, Context);
        }

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
        public virtual async Task DeleteAsync(T entity, IUnitTransaction tran)
        {
            await tran.RegisterAsync(async () =>
            {
                DbSet.Remove(entity);
                return await SaveChangesAsync();
            }, Context);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">实体</param>
        /// <param name="tran">事务</param>
        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, IUnitTransaction tran)
        {
            await tran.RegisterAsync(async () =>
            {
                DbSet.RemoveRange(entities);
                return await SaveChangesAsync();
            }, Context);
        }
    }
}
