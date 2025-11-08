using Microsoft.EntityFrameworkCore;
using OneForAll.Core.ORM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OneForAll.EFCore
{
    /// <summary>
    /// EFCore仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEFCoreRepository<T> : IEFCoreAddRepository<T>, IEFCoreUpdateRepository<T>, IEFCoreDeleteRepository<T> where T : class
    {
        DbContext Context { get; set; }
        DbSet<T> DbSet { get; set; }

        #region 查询

        /// <summary>
        /// 查询指定实体(追踪)
        /// </summary>
        /// <param name="primaryKeys">主键</param>
        /// <returns></returns>
        Task<T> FindAsync(params object[] primaryKeys);

        /// <summary>
        /// 查询指定实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 统计数量
        /// </summary>
        /// <param name="predicate">where条件</param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <returns>分页列表</returns>
        Task<IEnumerable<T>> GetPageAsync(int pageIndex, int pageSize);

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="predicate">条件</param>
        /// <returns>分页列表</returns>
        Task<IEnumerable<T>> GetPageAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns>列表</returns>
        Task<IEnumerable<T>> GetListAsync();

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>列表</returns>
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate);

        #endregion

        #region 存储过程

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        int StoreProcedure(string sql, params object[] parms);

        /// <summary>
        /// 执行查询存储过程
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="parms">参数</param>
        /// <returns>查询结果</returns>
        IEnumerable<T> QueryStoreProcedure(string sql, params object[] parms);

        #endregion

        /// <summary>
        /// 保存变更
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 保存变更
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}
