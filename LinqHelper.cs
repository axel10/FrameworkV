using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace Root
{
    public static class LinqHelper
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> q, string condition)
        {
            string[] conditions = condition.Split(',');

            if (conditions.Length == 0)
            {
                return (IOrderedQueryable<T>) q;
            }

            IOrderedQueryable<T> res = null;

            for (int i = 0; i < conditions.Length; i++)
            {
                string[] strings = conditions[i].Split(" ");
                var fieldName = strings[0];
                var direction = strings.Length > 1 ? strings[1] : "asc";

                var param = Expression.Parameter(typeof(T), "p");
                var prop = Expression.Property(param, fieldName);
                var exp = Expression.Lambda(prop, param);

                string method;

                if (i == 0)
                {
                    method = direction.ToLower() == "desc" ? "OrderBy" : "OrderByDescending";
                }
                else
                {
                    method = direction.ToLower() == "desc" ? "ThenBy" : "ThenByDescending";
                }

                Type[] types = {q.ElementType, exp.Body.Type};
                var mce = i == 0
                    ? Expression.Call(typeof(Queryable), method, types, q.Expression, exp)
                    : Expression.Call(typeof(Queryable), method, types, res.Expression, exp);

                if (conditions.Length == 1)
                {
                    return (IOrderedQueryable<T>) q.Provider.CreateQuery<T>(mce);
                }

                res = i == 0
                    ? (IOrderedQueryable<T>) q.Provider.CreateQuery<T>(mce)
                    : (IOrderedQueryable<T>) res.Provider.CreateQuery<T>(mce);
            }

            return res;
        }

        public static IQueryable<T> ToRead<T>(this IQueryable<T> list, ReadOptions readOptions,
            ControllerBase controller)
        {
            if (!readOptions.PageSize.HasValue)
            {
                readOptions.PageSize = 10;
            }

            if (!readOptions.PageNo.HasValue)
            {
                readOptions.PageNo = 1;
            }

            controller.HttpContext.Items.Add("Total", list.Count());

            return list.Skip((readOptions.PageNo.Value - 1) * readOptions.PageSize.Value)
                .Take(readOptions.PageSize.Value);
        }

        public static IQueryable<T> ToRead<T>(this IQueryable<T> list, ReadOptions readOptions)
        {
            if (!readOptions.PageSize.HasValue)
            {
                readOptions.PageSize = 10;
            }

            if (!readOptions.PageNo.HasValue)
            {
                readOptions.PageNo = 1;
            }

            return list.Skip((readOptions.PageNo.Value - 1) * readOptions.PageSize.Value)
                .Take(readOptions.PageSize.Value);
        }

        /*        public static IEnumerable<T> ToRead<T>(this IEnumerable<T> list, ReadOptions readOptions,
                    ControllerBase controller, bool isWillCount = false)
                {
                    if (!readOptions.PageSize.HasValue)
                    {
                        readOptions.PageSize = 10;
                    }

                    if (!readOptions.PageNo.HasValue)
                    {
                        readOptions.PageNo = 1;
                    }

                    if (isWillCount)
                    {
                        controller.HttpContext.Items.Add("Total", list.Count());
                    }

                    return list.Skip((readOptions.PageNo.Value - 1) * readOptions.PageSize.Value)
                        .Take(readOptions.PageSize.Value);
                }*/

        public static IQueryable<T> ToRead<T>(this IQueryable<T> list, ControllerBase controller)
        {
            controller.HttpContext.Items.Add("Total", list.Count());
            return list;
        }
    }
}