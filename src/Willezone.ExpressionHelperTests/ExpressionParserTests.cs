using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Willezone.ExpressionHelper;

namespace willezone.ExpressionHelperTests
{
    [TestClass]
    public class ExpressionParserTests
    {
        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestClass
        {
#pragma warning disable 649
            public int Field;
#pragma warning restore 649

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Property { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public TestClass SubProperty { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public IEnumerable<TestClass> Classes { get; set; }

            // ReSharper disable once MemberCanBeMadeStatic.Local
            public string Select()
            {
                return String.Empty;
            }
        }

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
