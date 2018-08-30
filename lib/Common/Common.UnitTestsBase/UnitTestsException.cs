using System;

namespace Common.UnitTestsBase
{
    public class UnitTestsException : Exception
    {
        public UnitTestsException(string message)
            : base(message)
        {
            // Left blank intentionally
        }
    }
}