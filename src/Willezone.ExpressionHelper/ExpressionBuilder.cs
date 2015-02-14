using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Willezone.ExpressionHelper
{
    public class ExpressionBuilder
    {
        public static Func<T, object> CreateMemberAccess<T>(string field)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(field), "Field must have a value");

            var type = typeof(T);

            MemberInfo memberInfo = 
                type.GetFields(BindingFlags.Public | BindingFlags.Instance).Cast<MemberInfo>()
                .Concat(type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                .FirstOrDefault(m => m.Name == field);

            if (memberInfo == null)
            {
                throw new MissingMemberException(type.Name, field);
            }

            var parameter = Expression.Parameter(typeof (T), "Model");
            var memberExpression = Expression.MakeMemberAccess(parameter, memberInfo);
            var convertExpression = Expression.Convert(memberExpression, typeof (object));

            var lambda = Expression.Lambda<Func<T, object>>(convertExpression, parameter);
            return lambda.Compile();
        }
    }
}