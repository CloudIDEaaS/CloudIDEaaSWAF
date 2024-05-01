using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class TestKindAttribute : Attribute
    {
        public TestKind TestKind { get; }

        public TestKindAttribute(TestKind testKind)
        {
            this.TestKind = testKind;
        }
    }
}
