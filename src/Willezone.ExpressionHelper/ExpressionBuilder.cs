using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Willezone.ExpressionHelper
{
    public class ExpressionBuilder
    {
        public static Func<T, object> CreateMemberAccess<T>(string memberPath)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(memberPath), "memberPath must have a value");
            var memberPathParts = SplitMemberPath(memberPath);

            var parameter = Expression.Parameter(typeof(T), "Model");
            Type parentType = typeof(T);
            Expression parentExpression = parameter;
             
            for (int i = 0; i < memberPathParts.Length; i++)
            {
                MemberInfo memberInfo =
                    parentType.GetFields(BindingFlags.Public | BindingFlags.Instance).Cast<MemberInfo>()
                    .Concat(parentType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    .FirstOrDefault(m => m.Name == memberPathParts[i]);

                if (memberInfo == null)
                {
                    throw new MissingMemberException(parentType.Name, memberPathParts[i]);
                }

                parentType = memberInfo.DeclaringType;
                parentExpression = Expression.MakeMemberAccess(parentExpression, memberInfo);
            }

            var convertExpression = Expression.Convert(parentExpression, typeof(object));
            var lambda = Expression.Lambda<Func<T, object>>(convertExpression, parameter);

            return lambda.Compile();
        }

        private static string[] SplitMemberPath(string memberPath)
        {
            return memberPath.Split('.');
        }
    }
}