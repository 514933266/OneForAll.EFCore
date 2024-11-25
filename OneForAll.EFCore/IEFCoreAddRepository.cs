using OneForAll.Core.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OneForAll.EFCore
{
    /// <summary>
    /// 添加
    /// </summary>
    public interface IEFCoreAddRepository<T> where T : class, new()
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        int Add(T entity);

        /// <summary>
        /// 添加（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        int AddRange(IEnumerable<T> entities);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        int BulkInsert(IList<T> entities);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">事务</param>
        void Add(T entity, IUnitTransaction tran);

        /// <summary>
        /// 添加（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <param name="tran">事务</param>
        void AddRange(IEnumerable<T> entities, IUnitTransaction tran);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="predicate">条件</param>
        /// <param name="tran">事务</param>
        void AddIfNotExists(T entity, Expression<Func<T, bool>> predicate, IUnitTransaction tran);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        Task<int> AddAsync(T entity);

        /// <summary>
        /// 添加（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        Task<int> AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        Task<int> BulkInsertAsync(IList<T> entities);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">事务</param>
        Task AddAsync(T entity, IUnitTransaction tran);

        /// <summary>
        /// 添加（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <param name="tran">事务</param>
        Task AddRangeAsync(IEnumerable<T> entities, IUnitTransaction tran);

    }
}
