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
        public static WireMockServer TwitterLoginStub;

        private static void StartFakeTwitterLogin()
        {
            TwitterLoginStub = WireMockServer.Start( new WireMockServerSettings
            {
                Urls = new[] {"http://localhost:5000"},
                StartAdminInterface = true
            });
        }
        private static void StartFakeTwitter()
        {
            TwitterStub = WireMockServer.Start( new WireMockServerSettings
            {
                Urls = new[] {"http://localhost:5001"},
                StartAdminInterface = true
            });
        }

        [BeforeTestRun]
        private static void BeforeTestRun()
        {
            StartFakeTwitterLogin();
            StartFakeTwitter();
        }

        [BeforeScenario]
        private static void BeforeScenario(ScenarioContext currentScenario)
        {
            Console.WriteLine($"Starting Scenario: {currentScenario.ScenarioInfo.Title}");
            TwitterLoginStub.Reset();
            TwitterStub.Reset();

        }

        [AfterTestRun]
        private static void AfterTestRun()
        {
            // TwitterLoginStub.Stop();
            // TwitterStub.Stop();
        }
    }


    
}