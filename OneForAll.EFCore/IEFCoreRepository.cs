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
        Task<T> FindAsync(params object[] primaryKeys);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetListAsync();
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate);
        #endregion

        #region 添加
        Task<int> AddAsync(T entity);
        Task<int> AddRangeAsync(IEnumerable<T> entities);

        Task<int> BulkInsertAsync(IList<T> entities);
        void Add(T entity, IUnitTransaction tran);

        void AddList(IEnumerable<T> entities, IUnitTransaction tran);

        void AddIfNotExists(T entity, Expression<Func<T, bool>> predicate, IUnitTransaction tran);

        #endregion

        #region 修改

        Task<int> UpdateAsync(T entity);

        Task<int> UpdateAsync(IEnumerable<T> entities);

        Task<int> BulkUpdateAsync(IList<T> entities);

        void Update(T entity, IUnitTransaction tran);
        #endregion

        #region 删除
        Task<int> DeleteAsync(T entity);

        Task<int> DeleteRangeAsync(IEnumerable<T> entities);

        Task<int> BulkDeleteAsync(IList<T> entities);

        void Delete(T entity, IUnitTransaction tran);

        void Delete(Expression<Func<T, bool>> predicate, IUnitTransaction tran);

        void BulkDelete(Expression<Func<T, bool>> predicate, IUnitTransaction tran);
        #endregion

        #region 存储过程
        int StoreProcedure(string sql, params object[] parms);

        IEnumerable<T> QueryStoreProcedure(string sql, params object[] parms);
        #endregion
        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
