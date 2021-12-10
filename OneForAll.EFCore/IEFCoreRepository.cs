using Microsoft.EntityFrameworkCore;
using OneForAll.Core.ORM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OneForAll.EFCore
{
    public interface IEFCoreRepository<T> where T : class
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

        #region 添加

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
        void Add(T entity, IUnitTransaction tran);

        /// <summary>
        /// 添加（批量）
        /// </summary>
        /// <param name="entities">实体</param>
        /// <param name="tran">事务</param>
        void AddList(IEnumerable<T> entities, IUnitTransaction tran);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="predicate">条件</param>
        /// <param name="tran">事务</param>
        void AddIfNotExists(T entity, Expression<Func<T, bool>> predicate, IUnitTransaction tran);

        #endregion

        #region 修改

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
        Task<int> UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        Task<int> BulkUpdateAsync(IList<T> entities);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">事务</param>
        void Update(T entity, IUnitTransaction tran);
        #endregion

        #region 删除

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
        void Delete(T entity, IUnitTransaction tran);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="tran">事务</param>
        void Delete(Expression<Func<T, bool>> predicate, IUnitTransaction tran);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="tran">事务</param>
        void BulkDelete(Expression<Func<T, bool>> predicate, IUnitTransaction tran);
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
