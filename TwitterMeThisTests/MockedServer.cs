using System;
using WireMock.Server;

namespace TwitterMeThisTests
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
