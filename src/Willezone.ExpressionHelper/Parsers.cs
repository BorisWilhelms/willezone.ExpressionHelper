using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace Willezone.ExpressionHelper
{
    /// <summary>
    /// Contains all parser methods.
    /// </summary>
    internal static class Parsers
    {
        private static readonly Dictionary<ExpressionType, Func<Expression, string>> SupportedParsers;

        static Parsers()
        {
            SupportedParsers = new Dictionary<ExpressionType, Func<Expression, string>>
            {
                { ExpressionType.MemberAccess, ParseMemberExpression },
                { ExpressionType.Call, ParseMethodCallExpression },
                { ExpressionType.Lambda, ParseLambdaExpression },
                { ExpressionType.Convert, ParseConvertExpression },
                { ExpressionType.Parameter, Noop }
            };
        }

        /// <summary>
        ///     Gets a parser for the given <paramref name="expressionType" />.
        /// </summary>
        /// <param name="expressionType">The expression type to parse.</param>
        /// <exception cref="NotSupportedException">Thrown when a not supported ExpressionType is detected.</exception>
        /// <returns>A function delegate that takes an expression and returns a string.</returns>
        internal static Func<Expression, string> GetParser(ExpressionType expressionType)
        {
            Contract.Ensures(Contract.Result<Func<Expression, string>>() != null);
            Func<Expression, string> parser;

            if (!SupportedParsers.TryGetValue(expressionType, out parser))
            {
                throw new NotSupportedException(String.Format(
                    "The expression of type {0} is not supported",
                    expressionType));
            }

            Contract.Assume(parser != null);
            return parser;
        }

        private static string ParseMemberExpression(Expression expression)
        {
            Contract.Requires<NotSupportedException>(expression is MemberExpression,
                "Expression must be of type MemberExpression");
            Contract.Requires(((MemberExpression)expression).Expression != null);

            var memberExpression = (MemberExpression)expression;
            var result = memberExpression.Member.Name;

            var parser = GetParser(memberExpression.Expression.NodeType);
            result = parser(memberExpression.Expression) + "." + result;

            return result.TrimStart('.');
        }

        private static string ParseMethodCallExpression(Expression expression)
        {
            Contract.Requires<NotSupportedException>(expression is MethodCallExpression,
                "Expression must be of type MethodCallExpression");
            Contract.Requires<NotSupportedException>(IsMethodSupported(((MethodCallExpression)expression).Method),
                "Only Enumerable.Select can be used");

            var methodCallExpression = (MethodCallExpression)expression;
            var argumentNames = new List<string>();
            foreach (var argument in methodCallExpression.Arguments)
            {
                Contract.Assume(argument != null);
                var parser = GetParser(argument.NodeType);
                argumentNames.Add(parser(argument));
            }

            return String.Join(".", argumentNames.ToArray());
        }

        private static string ParseLambdaExpression(Expression expression)
        {
            Contract.Requires<NotSupportedException>(expression is LambdaExpression,
                "Expression must be of type LambdaExpression");

            var lambdaExpression = (LambdaExpression)expression;
            var parser = GetParser(lambdaExpression.Body.NodeType);

            return parser(lambdaExpression.Body);
        }

        private static string ParseConvertExpression(Expression expression)
        {
            Contract.Requires<NotSupportedException>(expression is UnaryExpression,
                "Expression must be of type UnaryExpression");

            var unaryExpression = (UnaryExpression)expression;
            var parser = GetParser(unaryExpression.Operand.NodeType);

            return parser(unaryExpression.Operand);
        }

        private static string Noop(Expression expression)
        {
            Trace.WriteLine("Noop invoked!");
            return String.Empty;
        }

        [Pure]
        private static bool IsMethodSupported(MethodInfo methodInfo)
        {
            Contract.Requires(methodInfo != null);

            if (methodInfo.Name == "Select" &&
                methodInfo.DeclaringType != null &&
                methodInfo.DeclaringType.FullName == "System.Linq.Enumerable")
            {
                return true;
            }

            return false;
        }
    }
}