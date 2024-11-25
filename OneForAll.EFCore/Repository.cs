using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace OneForAll.EFCore
{
    /// <summary>
    /// 仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class Repository<T> : IEFCoreRepository<T> where T : class, new()
    {
        /// <summary>
        /// 当前连接
        /// </summary>
        public DbContext Context { get; set; }

        /// <summary>
        /// 当前表
        /// </summary>
        public DbSet<T> DbSet { get; set; }

        /// <summary>
        /// 当前只读连接
        /// </summary>
        public IEFCoreSelectRepository<T> Readonly { get; set; }

        /// <summary>
        /// 只读连接集合
        /// </summary>
        public List<IEFCoreSelectRepository<T>> Readonlys { get; set; } = new List<IEFCoreSelectRepository<T>>();

        public Repository(DbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public Repository(List<DbContext> contexts)
        {
            if (contexts != null && contexts.Count > 0)
            {
                // 取第一条作为主库
                Context = contexts.First();
                DbSet = Context.Set<T>();
                // 其余为从库
                for (int i = 1; i < contexts.Count; i++)
                {
                    var repository = new Repository<T>(contexts[i]) as IEFCoreSelectRepository<T>;
                    if (Readonly == null)
                        Readonly = repository;
                    Readonlys.Add(repository);
                }
            }
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int Execute(string sql, params object[] parms)
        {
            return Context.Database.ExecuteSqlRaw(sql, parms);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        /// <summary>
        /// 保存（异步）
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        /// <summary>
        /// 回收
        /// </summary>
        public void Dispose()
        {
            Context.Dispose();
        }

        #region 条件语句构建

        /// <summary>
        /// 相等
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <param name="key"></param>
        /// <returns></returns>

        protected Expression<Func<T, bool>> Equal<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            return BuildBinaryWhere(selector, key, Expression.Equal);
        }

        /// <summary>
        /// 不等于
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected Expression<Func<T, bool>> NotEqual<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            return BuildBinaryWhere(selector, key, Expression.NotEqual);
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected Expression<Func<T, bool>> GreaterThan<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            return BuildBinaryWhere(selector, key, Expression.GreaterThan);
        }

        /// <summary>
        /// 大于等于
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected Expression<Func<T, bool>> GreaterThanOrEqual<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            return BuildBinaryWhere(selector, key, Expression.GreaterThanOrEqual);
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected Expression<Func<T, bool>> LessThan<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            return BuildBinaryWhere(selector, key, Expression.LessThan);
        }

        /// <summary>
        /// 小于等于
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected Expression<Func<T, bool>> LessThanOrEqual<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            return BuildBinaryWhere(selector, key, Expression.LessThanOrEqual);
        }

        /// <summary>
        /// where条件
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private Expression<Func<T, bool>> BuildBinaryWhere<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key, Func<Expression, Expression, Expression> func)
        {
            MemberExpression left = selector.Body as MemberExpression;
            ConstantExpression right = Expression.Constant(key, typeof(TProperty));
            return Expression.Lambda<Func<T, bool>>(func(left, right), selector.Parameters);
        }

        /// <summary>
        /// 包含
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected Expression<Func<T, bool>> Contains<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            MemberExpression left = selector.Body as MemberExpression;
            ConstantExpression right = Expression.Constant(key, typeof(TProperty));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(left, typeof(TProperty).GetMethod("Contains", new Type[] { typeof(TProperty) }), right), selector.Parameters);
        }

        /// <summary>
        /// 不包含
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected Expression<Func<T, bool>> NotContains<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            MemberExpression left = selector.Body as MemberExpression;
            ConstantExpression right = Expression.Constant(key, typeof(TProperty));
            return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Call(left, typeof(TProperty).GetMethod("Contains", new Type[] { typeof(TProperty) }), right)), selector.Parameters);
        }

        /// <summary>
        /// 范围比较
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        protected Expression<Func<T, bool>> Range<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key1, TProperty key2)
        {
            MemberExpression left = selector.Body as MemberExpression;
            ConstantExpression right1 = Expression.Constant(key1, typeof(TProperty));
            ConstantExpression right2 = Expression.Constant(key2, typeof(TProperty));
            Expression leftRange = Expression.GreaterThan(left, right1);
            Expression rightRange = Expression.LessThan(left, right2);
            return Expression.Lambda<Func<T, bool>>(Expression.And(leftRange, rightRange), selector.Parameters);
        }

        /// <summary>
        /// 范围比较（含等于）
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="selector"></param>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        protected Expression<Func<T, bool>> RangeOrEqual<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key1, TProperty key2)
        {
            MemberExpression left = selector.Body as MemberExpression;
            ConstantExpression right1 = Expression.Constant(key1, typeof(TProperty));
            ConstantExpression right2 = Expression.Constant(key2, typeof(TProperty));
            Expression leftRange = Expression.GreaterThanOrEqual(left, right1);
            Expression rightRange = Expression.LessThanOrEqual(left, right2);
            return Expression.Lambda<Func<T, bool>>(Expression.And(leftRange, rightRange), selector.Parameters);
        }

        #endregion
    }
}
