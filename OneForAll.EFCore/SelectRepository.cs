using Microsoft.EntityFrameworkCore;
using OneForAll.Core;
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
        /// 查询指定实体(追踪)
        /// </summary>
        /// <param name="primaryKeys">主键</param>
        /// <returns></returns>
        public virtual async Task<T> FindAsync(params object[] primaryKeys)
        {
            return await DbSet.FindAsync(primaryKeys);
        }

        /// <summary>
        /// 查询指定实体
        /// </summary>
        /// <param name="predicate">where条件</param>
        /// <returns></returns>
        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 统计数量
        /// </summary>
        /// <param name="predicate">where条件</param>
        /// <returns></returns>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.CountAsync(predicate);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <returns>分页列表</returns>
        public virtual async Task<IEnumerable<T>> GetPageAsync(int pageIndex, int pageSize)
        {
            return await DbSet
                .AsNoTracking()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="predicate">where条件</param>
        /// <returns>分页列表</returns>
        public virtual async Task<IEnumerable<T>> GetPageAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate)
        {
            return await DbSet
                .AsNoTracking()
                .Where(predicate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns>列表</returns>
        public virtual async Task<IEnumerable<T>> GetListAsync()
        {
            return await DbSet.ToListAsync();
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="predicate">where条件</param>
        /// <returns>列表</returns>
        public virtual async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }
    }
}
