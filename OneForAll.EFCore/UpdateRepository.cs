﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OneForAll.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OneForAll.Core.ORM;
using OneForAll.Core.Utility;
using OneForAll.Core.Extension;
using EFCore.BulkExtensions;
using System.Threading.Tasks;

namespace OneForAll.EFCore
{
    partial class Repository<T>
    {
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> UpdateAsync(T entity)
        {
            DbSet.Update(entity);
            return await SaveChangesAsync();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> UpdateAsync(IEnumerable<T> entities)
        {
            DbSet.UpdateRange(entities);
            return await SaveChangesAsync();
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities">实体</param>
        /// <returns>影响行数</returns>
        public virtual async Task<int> BulkUpdateAsync(IList<T> entities)
        {
            Context.BulkUpdate(entities);
            return await SaveChangesAsync();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="tran">事务</param>
        public virtual void Update(T entity, IUnitTransaction tran)
        {
            tran.Register(() =>
            {
                DbSet.Update(entity);
                return SaveChanges();
            }, Context);
        }
    }
}
