#!/bin/bash
set -e
set -u

export TWITTER_LOGIN_HOSTNAME="http://localhost:5000"

export CONSUMER_KEY="fakeconsumerkey"
export CONSUMER_SECRET="fakeconsumersecret"
export TWITTER_CLIENT_HOSTNAME="http://localhost:5001"
export SCREEN_NAME="realDonaldTrump"

echo Running acceptance tests

dotnet test AcceptanceTests