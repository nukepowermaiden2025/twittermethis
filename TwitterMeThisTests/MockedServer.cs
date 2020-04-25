using System;
using WireMock.Server;

namespace InvoicingTests
{
    public class MockedServer : IDisposable
    {
        public FluentMockServer MockServer { get; private set; }

        public MockedServer()
        {
            MockServer = FluentMockServer.Start();
        }

        public void Dispose()
        {
            MockServer.Stop();
        }
    }
}
