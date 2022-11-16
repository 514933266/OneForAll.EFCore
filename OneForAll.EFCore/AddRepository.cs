using OneForAll.Core.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using OneForAll.Core.DDD;

namespace OneForAll.EFCore
{
    public partial class Repository<T>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        public virtual int Add(T entity)
        {
            DbSet.Add(entity);
            return SaveChanges();
        }

        /// <summary>
        /// 添加（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        public virtual int AddRange(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);
            return SaveChanges();
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        public virtual int BulkInsert(IList<T> entities)
        {
            Context.BulkInsert(entities);
            return SaveChanges();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">事务</param>
        public virtual void Add(T entity, IUnitTransaction tran)
        {
            tran.Register(() =>
            {
                DbSet.Add(entity);
                return SaveChanges();
            }, Context);
        }

        /// <summary>
        /// 添加（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <param name="tran">事务</param>
        public virtual void AddRange(IEnumerable<T> entities, IUnitTransaction tran)
        {
            tran.Register(() =>
            {
                DbSet.AddRange(entities);
                return SaveChanges();
            }, Context);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="predicate">条件</param>
        /// <param name="tran">事务</param>
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

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> AddAsync(T entity)
        {
            DbSet.Add(entity);
            return await SaveChangesAsync();
        }

        /// <summary>
        /// 添加（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> AddRangeAsync(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);
            return await SaveChangesAsync();
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> BulkInsertAsync(IList<T> entities)
        {
            Context.BulkInsert(entities);
            return await SaveChangesAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">事务</param>
        public virtual async Task AddAsync(T entity, IUnitTransaction tran)
        {
            await tran.RegisterAsync(async () =>
            {
                await DbSet.AddAsync(entity);
                return await SaveChangesAsync();
            }, Context);
        }

        /// <summary>
        /// 添加（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <param name="tran">事务</param>
        public virtual async Task AddRangeAsync(IEnumerable<T> entities, IUnitTransaction tran)
        {
            await tran.RegisterAsync(async () =>
            {
                await DbSet.AddRangeAsync(entities);
                return await SaveChangesAsync();
            }, Context);
        }
    }
}
