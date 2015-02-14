using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Willezone.ExpressionHelper;

namespace willezone.ExpressionHelperTests
{
    [TestClass]
    public class ExpressionParserTests
    {
        [TestMethod]
        public void FirstLevelPropertyShouldReturnPropertyName()
        {
            string result = MemberExpressionParser.Parse<TestClass>(tc => tc.Property);
            result.Should().Be("Property");
        }

        [TestMethod]
        public void FirstLevelFieldShouldReturnFieldName()
        {
            string result = MemberExpressionParser.Parse<TestClass>(tc => tc.Field);
            result.Should().Be("Field");
        }

        [TestMethod]
        public void SecondLevelPropertyShouldReturnPropertyPathAndPropertyName()
        {
            string result = MemberExpressionParser.Parse<TestClass>(tc => tc.SubProperty.Property);
            result.Should().Be("SubProperty.Property");
        }

        [TestMethod]
        public void SecondLevelFieldShouldReturnPropertyPathAndFieldName()
        {
            string result = MemberExpressionParser.Parse<TestClass>(tc => tc.SubProperty.Field);
            result.Should().Be("SubProperty.Field");
        }

        [TestMethod]
        public void SelectOnPropertyShouldReturnPropertyPathAndFieldName()
        {
            string result = MemberExpressionParser.Parse<TestClass>(tc => tc.Classes.Select(t => t.Property));
            result.Should().Be("Classes.Property");
        }

        [TestMethod]
        public void ThrowNotSupportedExceptionIfNotEnumerableSelectIfCalled()
        {
            Action parseWithNotSupportedMethod = () => MemberExpressionParser.Parse<TestClass>(tc => tc.Select());
            parseWithNotSupportedMethod.ShouldThrow<NotSupportedException>();
        }
    }
}
