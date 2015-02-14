using System;
using System.Collections.Generic;

namespace willezone.ExpressionHelperTests
{
    internal class TestClass
    {
        public int Field;

        public string Property { get; set; }

        public TestClass SubProperty { get; set; }

        public IEnumerable<TestClass> Classes { get; set; }

        public string Select()
        {
            return String.Empty;
        }
    }

}
