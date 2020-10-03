using System;
using System.Linq.Expressions;

namespace XamarinUtility.Extensions
{
    public static class ExpressionExtension
    {
        public static string GetMemberName<T>(this Expression<T> expression)
        {
            return expression.Body switch
            {
                MemberExpression m => m.Member.Name,
                UnaryExpression u when u.Operand is MemberExpression m => m.Member.Name,
                _ => string.Empty
            };
        }
    }
}
