using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrchestratorTest
{
    class TestObject : IDisposable
    {
        public TestObject()
        {
            Console.WriteLine("Begin Test Object");
        }

        public void Dispose()
        {
            Console.WriteLine("End Test Object");
        }
    }
}
