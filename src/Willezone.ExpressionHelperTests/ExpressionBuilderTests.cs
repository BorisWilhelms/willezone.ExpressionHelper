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

        [TestMethod]
        public void CreateMemberAccessShouldWorkWitSubObjects()
        {
            var exp = ExpressionBuilder.CreateMemberAccess<TestClass>("SubProperty.Property");
            var testData = new TestClass() { SubProperty = new TestClass() { Property ="SubPropertyValue" } };
            exp(testData).Should().Be("SubPropertyValue");
        }
    }
}