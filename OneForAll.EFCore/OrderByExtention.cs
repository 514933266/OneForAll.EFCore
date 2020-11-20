using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace OneForAll.EFCore
{
    /// <summary>
    /// 扩展：排序
    /// </summary>
    public static class OrderByExtention
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName)
        {
            return OrderBy(query, propertyName, false);
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyName)
        {
            return OrderBy(query, propertyName, true);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, string sortType)
        {
            var isDesc = false;
            if (sortType.ToLower() == "desc") isDesc = true;
            return OrderBy(query, propertyName, isDesc);
        }

        public static IOrderedQueryable<T> OrderBy<T>(IQueryable<T> query, string propertyName, bool isDesc)
        {
            var property = typeof(T).GetProperty(propertyName);
            if (property == null) throw new Exception("property does not exist");

            var methodName = isDesc ? "OrderByDescending" : "OrderBy";

            var method = typeof(OrderByExtention)
                .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(typeof(T), property.PropertyType);
            return (IOrderedQueryable<T>)method.Invoke(null, new object[] { query, property });
        }
        private static IOrderedQueryable<T> OrderBy<T, TProp>(IQueryable<T> query, PropertyInfo property)
        {
            return query.OrderBy(GetLamba<T, TProp>(property));
        }
        private static IOrderedQueryable<T> OrderByDescending<T, TProp>(IQueryable<T> query, PropertyInfo property)
        {
            return query.OrderByDescending(GetLamba<T, TProp>(property));
        }

        private static Expression<Func<T, TProp>> GetLamba<T, TProp>(PropertyInfo property)
        {
            if (property.PropertyType != typeof(TProp)) throw new Exception("property type do not match");
            var arg = Expression.Parameter(typeof(T), "w");
            var lamba = Expression.Lambda<Func<T, TProp>>(Expression.Property(arg, property), arg);
            return lamba;
        }
    }
}
