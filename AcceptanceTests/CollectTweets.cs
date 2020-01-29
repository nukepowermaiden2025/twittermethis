using System;
using TechTalk.SpecFlow;

namespace AcceptanceTests
{
    [Binding]
    public class CollectTweetsSteps
    {
        [Given(@"the twitter api responds with")] 
        public void GivenTheTwitterApiRespondsWith( string tweetsJson)
        {

        }

        [When(@"I trigger the function CollectTweets")]
        public void WhenItriggerthefunctionCollectTweets()
        {
            
        }

        [Then(@"I expect my json file to have")]
        public void ThenIExpectMyJsonFileToHave(string designationAssignedJson)
        {

        }
    }
}