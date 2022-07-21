using ArquitecturaDemo.Shared.Helpers;
using ArquitecturaDemo.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace ArquitecturaDemo.BLL.Extensions
{
    public static class PaginationExtension
    {
        private static readonly MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        private static readonly MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        private static readonly MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
        private static readonly MethodInfo toStringMethod = typeof(int).GetMethod("ToString", new Type[] { });
        private static readonly MethodInfo dateToStringMethod = typeof(int).GetMethod("ToString", new[] { typeof(string) });

        public static async Task<InfinitePagination<T>> InfinitePaginate<T>(this IQueryable<T> query, int skip, int take, string order, int columnOrder, string generalFlter, List<FilterInfo> filters)
        {
            var paginationModel = new InfinitePagination<T>
            {
                RecordsTotal = query.Count()
            };

            try
            {
                var result = query.FilterQuery<T>(generalFlter, filters);
                paginationModel.RecordsFiltered = result.Count();
                paginationModel.Result = await result.Skip(skip).Take(take).CustomOrderBy(order, columnOrder).ToListAsync();
            }
            catch (Exception)
            {
                paginationModel.RecordsFiltered = query.Count();
                paginationModel.Result = await query.Skip(skip).Take(take).CustomOrderBy(order, columnOrder).ToListAsync();
            }

            return paginationModel;
        }

        private static IQueryable<T> FilterQuery<T>(this IQueryable<T> query, string generalFlter, List<FilterInfo> filters)
        {
            IQueryable<T> result = query;
            var props = typeof(T).GetProperties();
            var parameter = Expression.Parameter(typeof(T), Const.LambdaParameter);

            if (generalFlter != Const.EmptyParameter)
            {
                Expression resultExpression = null!;

                for (int i = 0; i < filters.Count; i++)
                {
                    var expression = GetExpression<T>(parameter, filters[i], props[i], generalFlter);
                    resultExpression = resultExpression == null ? expression : Expression.Or(resultExpression, expression);
                }

                var lambda = Expression.Lambda<Func<T, bool>>(resultExpression, parameter);
                result = query.Where(lambda);
            }

            if (filters.Count == props.Length)
            {
                for (int i = 0; i < filters.Count; i++)
                {
                    if (!filters[i].Value.Equals(Const.EmptyParameter))
                    {
                        var expression = GetExpression<T>(parameter, filters[i], props[i]);

                        var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter);
                        result = result.Where(lambda);
                    }
                }
            }

            return result;
        }

        private static IQueryable<T> CustomOrderBy<T>(this IQueryable<T> query, string order, int columnOrder)
        {
            var props = typeof(T).GetProperties();
            var parameter = Expression.Parameter(typeof(T), Const.LambdaParameter);

            var propertyAccess = Expression.MakeMemberAccess(parameter, props[columnOrder]);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            var command = order.Equals(Const.OrderByAsc) ? "OrderBy" : "OrderByDescending";

            Expression resultExpression = Expression.Call(typeof(Queryable), command, new Type[]
            {
                typeof(T),
                props[columnOrder].PropertyType
            }, query.Expression, Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<T>(resultExpression);
        }

        private static Expression GetExpression<T>(ParameterExpression param, FilterInfo filter, PropertyInfo prop, string? generalFilter = null)
        {
            var member = Expression.Property(param, prop.Name);
            var constant = Expression.Constant(!string.IsNullOrEmpty(generalFilter) ? generalFilter : filter.Value);

            if(filter.Operator != OperatorsEnum.Contains)
            {
                if (prop.PropertyType == typeof(DateTime))
                {
                    DateTime.TryParseExact(filter.Value, "d-M-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out DateTime date);
                    constant = Expression.Constant(date);
                }
                else
                {
                    var tmp = Convert.ChangeType(filter.Value, prop.PropertyType, CultureInfo.InvariantCulture);
                    constant = Expression.Constant(tmp);
                }
            }

            switch (filter.Operator)
            {
                case OperatorsEnum.Equals:
                    return Expression.Equal(member, constant);

                case OperatorsEnum.Contains:
                    {
                        if (prop.PropertyType == typeof(DateTime))
                        {
                            var format = Expression.Constant("d-M-yyyy");
                            var tmpMethod = Expression.Call(member, dateToStringMethod, format);
                            return Expression.Call(tmpMethod, containsMethod, constant);
                        }
                        else
                        {
                            var tmpMethod = Expression.Call(member, toStringMethod);
                            return Expression.Call(tmpMethod, containsMethod, constant);
                        }
                    }

                case OperatorsEnum.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case OperatorsEnum.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case OperatorsEnum.LessThan:
                    return Expression.LessThan(member, constant);

                case OperatorsEnum.LessThanOrEqualTo:
                    return Expression.LessThanOrEqual(member, constant);

                case OperatorsEnum.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);

                case OperatorsEnum.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
                case OperatorsEnum.NotEqual:
                    return Expression.NotEqual(member, constant);
                default:
                    break;
            }

            return null;
        }
    }
}