using OneForAll.Core.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneForAll.EFCore
{
    /// <summary>
    /// 更新
    /// </summary>
    public interface IEFCoreUpdateRepository<T> where T : class, new()
    {
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        int Update(T entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        int UpdateRange(IEnumerable<T> entities);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        int BulkUpdate(IList<T> entities);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">事务</param>
        void Update(T entity, IUnitTransaction tran);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entities">实体</param>
        /// <param name="tran">事务</param>
        void UpdateRange(IEnumerable<T> entities, IUnitTransaction tran);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        Task<int> UpdateAsync(T entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        Task<int> UpdateRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">事务</param>
        Task UpdateAsync(T entity, IUnitTransaction tran);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entities">实体</param>
        /// <param name="tran">事务</param>
        Task UpdateRangeAsync(IEnumerable<T> entities, IUnitTransaction tran);
    }
}
