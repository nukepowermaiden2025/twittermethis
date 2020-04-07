using System;
using TechTalk.SpecFlow;
using WireMock.Server;
using WireMock.Settings;

namespace AcceptanceTests
{
    [Binding]

    public static class Hooks
    {
        public static WireMockServer TwitterStub;

        [BeforeTestRun]
        private static void BeforeTestRun()
        {
            StartFakeTwitter();
        }

        private static void StartFakeTwitter()
        {
            TwitterStub = FluentMockServer.Start( new FluentMockServerSettings
            {
                Urls = new[] {"http://localhost:5000"},
                StartAdminInterface = true
            });
        }

        [BeforeScenario]
        private static void BeforeScenario(ScenarioContext currentScenario)
        {
            Console.WriteLine($"Starting Scenario: {currentScenario.ScenarioInfo.Title}");
            TwitterStub.Reset();
        }

        [AfterTestRun]
        private static void AfterTestRun()
        {
            TwitterStub.Stop();
        }
    }


    
}