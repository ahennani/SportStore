using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportStore.Models.Dtos;
using SportStore.Models.Entities;

namespace SportStore.Extensions
{
    public static class IQueryableExtentions
    {
        public static IQueryable<TEntity> OrderByCustome<TEntity>(this IQueryable<TEntity> entities, string sortOrder, string sortBy)
        {
            var type = typeof(TEntity);
            var proprety = type.GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            var exp1 = Expression.Parameter(type, "t");
            var exp2 = Expression.MakeMemberAccess(exp1, proprety); //Expression.Property(exp1, proprety);


            var lambda = Expression.Lambda(exp2, exp1);
            var result = Expression.Call
                (
                    typeof(Queryable),
                    sortOrder == "desc" ? "OrderByDescending" : "OrderBy",
                    new Type[] { type, proprety.PropertyType },
                    entities.Expression,
                    Expression.Quote(lambda)
                );

            return entities.Provider.CreateQuery<TEntity>(result);
        }
    }
}


//MethodInfo methodInfo = typeof(Queryable).GetMethods().Where(s => s.Name == "OrderBy").FirstOrDefault();