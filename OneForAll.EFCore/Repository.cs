using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace OneForAll.EFCore
{
    /// <summary>
    /// 仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class Repository<T> : IEFCoreRepository<T> where T : class
    {

        public virtual DbContext Context { get; set; }
        public virtual DbSet<T> DbSet { get; set; }

        public Repository(DbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public virtual int Execute(string sql, params object[] parms)
        {
            return Context.Database.ExecuteSqlRaw(sql, parms);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
        public void Dispose()
        {
            Context.Dispose();
        }

        #region 条件语句构建

        protected Expression<Func<T, bool>> Equal<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            return BuildBinaryWhere(selector, key, Expression.Equal);
        }

        protected Expression<Func<T, bool>> NotEqual<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            return BuildBinaryWhere(selector, key, Expression.NotEqual);
        }

        protected Expression<Func<T, bool>> GreaterThan<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            return BuildBinaryWhere(selector, key, Expression.GreaterThan);
        }

        protected Expression<Func<T, bool>> GreaterThanOrEqual<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            return BuildBinaryWhere(selector, key, Expression.GreaterThanOrEqual);
        }

        protected Expression<Func<T, bool>> LessThan<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            return BuildBinaryWhere(selector, key, Expression.LessThan);
        }

        protected Expression<Func<T, bool>> LessThanOrEqual<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            return BuildBinaryWhere(selector, key, Expression.LessThanOrEqual);
        }

        private Expression<Func<T, bool>> BuildBinaryWhere<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key, Func<Expression, Expression, Expression> func)
        {
            MemberExpression left = selector.Body as MemberExpression;
            ConstantExpression right = Expression.Constant(key, typeof(TProperty));
            return Expression.Lambda<Func<T, bool>>(func(left, right), selector.Parameters);
        }

        protected Expression<Func<T, bool>> Contains<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            MemberExpression left = selector.Body as MemberExpression;
            ConstantExpression right = Expression.Constant(key, typeof(TProperty));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(left, typeof(TProperty).GetMethod("Contains", new Type[] { typeof(TProperty) }), right), selector.Parameters);
        }

        protected Expression<Func<T, bool>> NotContains<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key)
        {
            MemberExpression left = selector.Body as MemberExpression;
            ConstantExpression right = Expression.Constant(key, typeof(TProperty));
            return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Call(left, typeof(TProperty).GetMethod("Contains", new Type[] { typeof(TProperty) }), right)), selector.Parameters);
        }

        protected Expression<Func<T, bool>> Range<TProperty>(Expression<Func<T, TProperty>> selector, TProperty key1, TProperty key2)
        {
            MemberExpression left = selector.Body as MemberExpression;
            ConstantExpression right1 = Expression.Constant(key1, typeof(TProperty));
            ConstantExpression right2 = Expression.Constant(key2, typeof(TProperty));
            Expression leftRange = Expression.GreaterThan(left, right1);
            Expression rightRange = Expression.LessThan(left, right2);
            return Expression.Lambda<Func<T, bool>>(Expression.And(leftRange, rightRange), selector.Parameters);
        }
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
