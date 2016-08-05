using System;

namespace Iris
{
    public class HermesTestingException : Exception
    {
        public HermesTestingException()
            : base("=== Test Error ===")
        {
        }
    }
}
