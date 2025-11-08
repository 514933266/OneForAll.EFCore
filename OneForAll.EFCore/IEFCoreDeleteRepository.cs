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
    /// 删除
    /// </summary>
    public interface IEFCoreDeleteRepository<T> where T : class, new()
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        int Delete(T entity);

        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        int DeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        int BulkDelete(IList<T> entities);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">事务</param>
        void Delete(T entity, IUnitTransaction tran);

        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <param name="tran">事务</param>
        /// <returns>影响行数</returns>
        void DeleteRange(IEnumerable<T> entities, IUnitTransaction tran);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        Task<int> DeleteAsync(T entity);

        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        Task<int> DeleteRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        Task<int> BulkDeleteAsync(IList<T> entities);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">事务</param>
        Task DeleteAsync(T entity, IUnitTransaction tran);

        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <param name="tran">事务</param>
        /// <returns>影响行数</returns>
        Task DeleteRangeAsync(IEnumerable<T> entities, IUnitTransaction tran);
    }
}
