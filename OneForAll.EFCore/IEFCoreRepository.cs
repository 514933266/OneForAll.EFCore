using Microsoft.EntityFrameworkCore;
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
    public interface IEFCoreRepository<T> : ISelectRepository<T>, IAddRepository<T>, IUpdateRepository<T>, IDeleteRepository<T> where T : class, new()
    {
        /// <summary>
        /// 当前连接
        /// </summary>
        DbContext Context { get; }

        /// <summary>
        /// 当前表
        /// </summary>
        DbSet<T> DbSet { get; }

        /// <summary>
        /// 当前只读连接
        /// </summary>
        ISelectRepository<T> Readonly { get; }

        /// <summary>
        /// 只读连接集合
        /// </summary>
        List<ISelectRepository<T>> Readonlys { get; }

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
