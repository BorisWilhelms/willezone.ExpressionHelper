using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Willezone.ExpressionHelper;

namespace willezone.ExpressionHelperTests
{
    [TestClass]
    public class ExpressionBuilderTests
    {
        [TestMethod]
        public void CreateMemberAccessShoudlWorkWithFields()
        {
            //This is some test
            var exp = ExpressionBuilder.CreateMemberAccess<TestClass>("Field");
            var testData = new TestClass() { Field = 123 };
            exp(testData).Should().Be(123);
        }

        [TestMethod]
        public void CreateMemberAccessShoudlWorkWithProperties()
        {
            var exp = ExpressionBuilder.CreateMemberAccess<TestClass>("Property");
            var testData = new TestClass() { Property = "PropertyValue" };
            exp(testData).Should().Be("PropertyValue");
        }
    }
}