using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Willezone.ExpressionHelper
{
    /// <summary>
    /// Contains methods to parse any kind of member like expressions.
    /// </summary>
    public class MemberExpressionParser
    {
        /// <summary>
        /// Parses the given <paramref name="expression"/> and returns the member path.
        /// </summary>
        /// <typeparam name="TIn">The type of the in parameter of the function. The type parameter exists only for better IntelliSense support.</typeparam>
        /// <param name="expression">The expression to parse. Must be an expression to select a member.</param>
        /// <exception cref="NotSupportedException">Thrown if the type of the expression or any part of it is not supported.</exception>
        /// <returns>The parsed member path.</returns>
        public static string Parse<TIn>(Expression<Func<TIn, object>> expression)
        {
            Contract.Requires<ArgumentNullException>(expression != null);

            return Parse((Expression)expression);
        }

        /// <summary>
        /// Parses the given <paramref name="expression"/> and returns the member path.
        /// </summary>
        /// <typeparam name="TIn">The type of the in parameter of the function. The type parameter exists only for better IntelliSense support.</typeparam>
        /// <typeparam name="TOut">The type of the result of the function. The type parameter exists only for better IntelliSense support.</typeparam>
        /// <param name="expression">The expression to parse. Must be an expression to select a member.</param>
        /// <exception cref="NotSupportedException">Thrown if the type of the expression or any part of it is not supported.</exception>
        /// <returns>The parsed member path.</returns>
        public static string Parse<TIn, TOut>(Expression<Func<TIn, TOut>> expression)
        {
            Contract.Requires<ArgumentNullException>(expression != null);

            return Parse((Expression)expression);
        }

        /// <summary>
        /// Parses the given <paramref name="expression"/> and returns the member path.
        /// </summary>
        /// <param name="expression">The expression to parse. Must be an expression to select a member.</param>
        /// <exception cref="NotSupportedException">Thrown if the type of the expression or any part of it is not supported.</exception>
        /// <returns>The parsed member path.</returns>
        public static string Parse(Expression expression)
        {
            Contract.Requires<ArgumentNullException>(expression != null);

            var parser = Parsers.GetParser(expression.NodeType);
            return parser(expression);
        }

        /// <summary>
        /// Tries to parse the given <paramref name="expression"/>.
        /// The return value indicates whether the parsing succeeded.
        /// </summary>
        /// <typeparam name="TIn">The type of the in parameter of the function. The type parameter exists only for better IntelliSense support.</typeparam>
        /// <param name="expression">The expression to parse. Must be an expression to select a member.</param>
        /// <param name="memberPath">
        ///     When this method returns, contains the member path equivalent to the expression contained in <paramref name="expression"/>, if the parsing succeeded, or <seealso cref="string.Empty"/> if the parsing failed. 
        ///     The parsing fails if the <paramref name="expression"/> parameter is null, or if the type of the expression or any part of it is not supported.
        /// </param>
        /// <returns><c>true</c> when the parsing was successful, otherwise <c>false</c></returns>
        public static bool TryParse<TIn>(Expression<Func<TIn, object>> expression, out string memberPath)
        {
            return TryParse((Expression)expression, out memberPath);
        }

        /// <summary>
        /// Tries to parse the given <paramref name="expression"/>.
        /// The return value indicates whether the parsing succeeded.
        /// </summary>
        /// <typeparam name="TIn">The type of the in parameter of the function. The type parameter exists only for better IntelliSense support.</typeparam>
        /// <typeparam name="TOut">The type of the result of the function. The type parameter exists only for better IntelliSense support.</typeparam>
        /// <param name="expression">The expression to parse. Must be an expression to select a member.</param>
        /// <param name="memberPath">
        ///     When this method returns, contains the member path equivalent to the expression contained in <paramref name="expression"/>, if the parsing succeeded, or <seealso cref="string.Empty"/> if the parsing failed. 
        ///     The parsing fails if the <paramref name="expression"/> parameter is null, or if the type of the expression or any part of it is not supported.
        /// </param>
        /// <returns><c>true</c> when the parsing was successful, otherwise <c>false</c></returns>
        public static bool TryParse<TIn, TOut>(Expression<Func<TIn, TOut>> expression, out string memberPath)
        {
            return TryParse((Expression)expression, out memberPath);
        }

        /// <summary>
        /// Tries to parse the given <paramref name="expression"/>.
        /// The return value indicates whether the parsing succeeded.
        /// </summary>
        /// <param name="expression">The expression to parse. Must be an expression to select a member.</param>
        /// <param name="memberPath">
        ///     When this method returns, contains the member path equivalent to the expression contained in <paramref name="expression"/>, if the parsing succeeded, or <seealso cref="string.Empty"/> if the parsing failed. 
        ///     The parsing fails if the <paramref name="expression"/> parameter is null, or if the type of the expression or any part of it is not supported.
        /// </param>
        /// <returns><c>true</c> when the parsing was successful, otherwise <c>false</c></returns>
        public static bool TryParse(Expression expression, out string memberPath)
        {
            memberPath = String.Empty;
            if (expression == null)
            {
                return false;
            }

            try
            {
                memberPath = Parse(expression);
                return true;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }
    }
}