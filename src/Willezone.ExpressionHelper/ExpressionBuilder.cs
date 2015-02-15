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

            MemberExpression memberExpression = null;
            var parameter = Expression.Parameter(typeof(T), "Model");

            for (int i = 0; i < memberPathParts.Length; i++)
            {
                if (i == 0)
                {
                    var type = typeof(T);

                    MemberInfo memberInfo =
                        type.GetFields(BindingFlags.Public | BindingFlags.Instance).Cast<MemberInfo>()
                        .Concat(type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        .FirstOrDefault(m => m.Name == memberPathParts[i]);

                    if (memberInfo == null)
                    {
                        throw new MissingMemberException(type.Name, memberPath);
                    }

                    memberExpression = Expression.MakeMemberAccess(parameter, memberInfo);
                }
                else
                {
                    var type = memberExpression.Member.DeclaringType;

                    MemberInfo memberInfo =
                        type.GetFields(BindingFlags.Public | BindingFlags.Instance).Cast<MemberInfo>()
                        .Concat(type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        .FirstOrDefault(m => m.Name == memberPathParts[i]);

                    if (memberInfo == null)
                    {
                        throw new MissingMemberException(type.Name, memberPath);
                    }

                    memberExpression = Expression.MakeMemberAccess(memberExpression, memberInfo);
                }
            }

            var convertExpression = Expression.Convert(memberExpression, typeof(object));

            var lambda = Expression.Lambda<Func<T, object>>(convertExpression, parameter);
            return lambda.Compile();
        }

        private static string[] SplitMemberPath(string memberPath)
        {
            return memberPath.Split('.');
        }
    }
}