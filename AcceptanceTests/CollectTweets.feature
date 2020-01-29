Feature: Collects tweets from the tweets

Scenario: Collects tweets
   Given the twitter api responds with 
   """
   {
      "created_at": "Wed Oct 10 20:19:24 +0000 2018",
      "id": 1050118621198921700,
      "id_str": "1050118621198921728",
      "text": "Count all political speech as negative",
      "user": {},
      "entities": {}
   }
   """
   When I trigger the function CollectTweets
   Then I expect my json file to have
   """
   [
      {
         "created_at": "Wed Oct 10 20:19:24 +0000 2018",
         "id": 1050118621198921700,
         "text": "Count all political speech as negative",
         "user": {},
         "is_positive" : false
      }
   ]
   """