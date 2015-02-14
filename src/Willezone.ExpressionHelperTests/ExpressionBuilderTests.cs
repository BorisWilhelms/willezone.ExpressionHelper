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
            var exp = ExpressionBuilder.CreateMemberAccess<TestClass>("Field");
            var testData = new TestClass() {Field = 123};
            exp(testData).Should().Be(123);
        }
    }
}