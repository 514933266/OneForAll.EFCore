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
            var properties = new List<PropertyInfo>();
            var methodName = isDesc ? "OrderByDescending" : "OrderBy";
            var property = typeof(T).GetProperty(propertyName);
            if (property == null)
            {
                var deeps = propertyName.Split('.');

                property = typeof(T).GetProperty(deeps[0]);
                properties.Add(property);
                if (property == null) throw new Exception("property type do not match");
                for (int i = 1; i < deeps.Length; i++)
                {
                    property = property.PropertyType.GetProperty(deeps[i]);
                    properties.Add(property);
                }
            }
            else
            {
                properties.Add(property);
            }
            var method = typeof(OrderByExtention)
            .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static)
            .MakeGenericMethod(typeof(T), property.PropertyType);
            return (IOrderedQueryable<T>)method.Invoke(null, new object[] { query, properties });
        }

        private static IOrderedQueryable<T> OrderBy<T, TProp>(IQueryable<T> query, IEnumerable<PropertyInfo> properties)
        {
            return query.OrderBy(GetLamba<T, TProp>(properties));
        }
        private static IOrderedQueryable<T> OrderByDescending<T, TProp>(IQueryable<T> query, IEnumerable<PropertyInfo> properties)
        {
            return query.OrderByDescending(GetLamba<T, TProp>(properties));
        }

        private static Expression<Func<T, TProp>> GetLamba<T, TProp>(IEnumerable<PropertyInfo> properties)
        {
            var count = properties.Count();
            var arg = Expression.Parameter(typeof(T), "w");
            var lamba = Expression.Property(arg, properties.First());
            for (int i = 1; i < count; i++)
            {
                lamba = Expression.Property(lamba, properties.ElementAt(i));
            }
            return Expression.Lambda<Func<T, TProp>>(lamba, arg);
        }
    }
}