using System;
using System.CodeDom;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Willezone.ExpressionHelper;

namespace willezone.ExpressionHelperTests
{
    [TestClass]
    public class ExpressionBuilderTests
    {
        [TestMethod]
        public void CreateMemberAccessShouldWorkWithFields()
        {
            Func<TestClass, object> resultExpression = ExpressionBuilder.CreateMemberAccess<TestClass>("Field");

            var testData = new TestClass() {Field = 123};
            resultExpression(testData).Should().Be(123);
        }

        [TestMethod]
        public void CreateMemberAccessShouldWorkWithPropertoes()
        {
            Func<TestClass, object> resultExpression = ExpressionBuilder.CreateMemberAccess<TestClass>("Property");

            var testData = new TestClass() { Property = "PropertyValue" };
            resultExpression(testData).Should().Be("PropertyValue");
        }
    }
}