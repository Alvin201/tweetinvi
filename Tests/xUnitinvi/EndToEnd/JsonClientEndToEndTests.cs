using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    public class JsonClientTests : TweetinviTest
    {
        public JsonClientTests(ITestOutputHelper logger) : base(logger)
        {
        }

        private void TestSerializer<TFrom, TTo>(TFrom input, Action<TTo> verify)
            where TFrom : class
            where TTo : class
        {
            _logger.WriteLine($"Conversion of : {typeof(TFrom).Name} <--> {typeof(TTo).Name}");

            var json = _tweetinviClient.Json.Serialize(input);
            var deserializedObject = _tweetinviClient.Json.Deserialize<TTo>(json);

            var inputArray = new[] {input, input};
            var arrayJson = _tweetinviClient.Json.Serialize(inputArray);
            var deserializedArray = _tweetinviClient.Json.Deserialize<TTo[]>(arrayJson);

            verify?.Invoke(deserializedObject);

            Assert.Equal(deserializedArray.Length, 2);
            verify?.Invoke(deserializedArray[1]);
        }

        [Fact]
        public async Task Users()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var user = await _tweetinviClient.Users.GetUser("tweetinviapi");
            var verifier = new Action<IUserIdentifier>(identifier => { Assert.Equal(user.Id, identifier.Id); });

            TestSerializer<IUser, IAuthenticatedUser>(user, verifier);
            TestSerializer<IUser, IUser>(user, verifier);
            TestSerializer<IUser, IUserDTO>(user, verifier);
            TestSerializer<IUserDTO, IUserDTO>(user.UserDTO, verifier);
            TestSerializer<IUserDTO, IUser>(user.UserDTO, verifier);
        }

        [Fact]
        public async Task AuthenticatedUsers()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var user = await _tweetinviClient.Users.GetAuthenticatedUser();
            var verifier = new Action<IUserIdentifier>(identifier => { Assert.Equal(user.Id, identifier.Id); });

            TestSerializer<IAuthenticatedUser, IAuthenticatedUser>(user, verifier);
            TestSerializer<IAuthenticatedUser, IUser>(user, verifier);
            TestSerializer<IAuthenticatedUser, IUserDTO>(user, verifier);
            TestSerializer<IUser, IAuthenticatedUser>(user, verifier);
            TestSerializer<IUserDTO, IAuthenticatedUser>(user.UserDTO, verifier);
        }

        [Fact]
        public async Task Tweets()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var tweet = await _tweetinviClient.Tweets.GetTweet(979753598446948353);
            var verifier = new Action<ITweetIdentifier>(identifier => { Assert.Equal(tweet.Id, identifier.Id); });

            TestSerializer<ITweet, ITweet>(tweet, verifier);
            TestSerializer<ITweet, ITweetDTO>(tweet, verifier);
            TestSerializer<ITweetDTO, ITweet>(tweet.TweetDTO, verifier);
            TestSerializer<ITweetDTO, ITweetDTO>(tweet.TweetDTO, verifier);
        }

        [Fact]
        public async Task Message()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var message = await _tweetinviClient.Messages.PublishMessage("How are you doing?", EndToEndTestConfig.TweetinviTest.UserId);
            await message.Destroy();

            TestSerializer<IMessage, IMessage>(message, deserializedMessage => { Assert.Equal(message.Text, deserializedMessage.Text); });

            TestSerializer<IMessage, IMessageEventDTO>(message, deserializedMessage => { Assert.Equal(message.Text, deserializedMessage.MessageCreate.MessageData.Text); });

            _logger.WriteLine($"Conversion of : {typeof(IMessage).Name} <--> {typeof(IMessageEventWithAppDTO).Name}");

            var json = _tweetinviClient.Json.Serialize<IMessage, IMessageEventWithAppDTO>(message);
            var deserializedObject = _tweetinviClient.Json.Deserialize<IMessageEventWithAppDTO>(json);

            Assert.Equal(message.Text, deserializedObject.MessageEvent.MessageCreate.MessageData.Text);
        }

        [Fact]
        public async Task TwitterLists()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var list = await _tweetinviClient.Lists.CreateList("THE_LIST");
            await _tweetinviClient.Lists.DestroyList(list);

            var verifier = new Action<ITwitterListIdentifier>(identifier => { Assert.Equal(list.Id, identifier.Id); });

            TestSerializer<ITwitterList, ITwitterList>(list, verifier);
            TestSerializer<ITwitterList, ITwitterListDTO>(list, verifier);
            TestSerializer<ITwitterListDTO, ITwitterListDTO>(list.TwitterListDTO, verifier);
            TestSerializer<ITwitterListDTO, ITwitterList>(list.TwitterListDTO, verifier);
        }

        [Fact]
        public async Task AccountSettings()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var accountSettings = await _tweetinviClient.AccountSettings.GetAccountSettings();

            TestSerializer<IAccountSettings, IAccountSettings>(accountSettings, settings =>
            {
                Assert.Equal(accountSettings.TimeZone.Name, settings.TimeZone.Name);
            });
            TestSerializer<IAccountSettings, IAccountSettingsDTO>(accountSettings, settings =>
            {
                Assert.Equal(accountSettings.TimeZone.Name, settings.TimeZone.Name);
            });
            TestSerializer<IAccountSettingsDTO, IAccountSettingsDTO>(accountSettings.AccountSettingsDTO, settings =>
            {
                Assert.Equal(accountSettings.TimeZone.Name, settings.TimeZone.Name);
            });
            TestSerializer<IAccountSettingsDTO, IAccountSettings>(accountSettings.AccountSettingsDTO, settings =>
            {
                Assert.Equal(accountSettings.TimeZone.Name, settings.TimeZone.Name);
            });
        }

        [Fact]
        public void Credentials()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var twitterCredentials = new TwitterCredentials("A", "B", "C", "D")
            {
                BearerToken = "E"
            };

            TestSerializer<ITwitterCredentials, ITwitterCredentials>(twitterCredentials, creds =>
            {
                Assert.Equal(twitterCredentials.AccessToken, creds.AccessToken);
                Assert.Equal(twitterCredentials.ConsumerKey, creds.ConsumerKey);
                Assert.Equal(twitterCredentials.BearerToken, creds.BearerToken);
            });

            TestSerializer<ITwitterCredentials, IConsumerCredentials>(twitterCredentials, creds =>
            {
                Assert.Equal(twitterCredentials.ConsumerKey, creds.ConsumerKey);
                Assert.Equal(twitterCredentials.BearerToken, creds.BearerToken);
            });

            TestSerializer<IConsumerCredentials, IConsumerCredentials>(twitterCredentials, creds =>
            {
                Assert.Equal(twitterCredentials.ConsumerKey, creds.ConsumerKey);
                Assert.Equal(twitterCredentials.BearerToken, creds.BearerToken);
            });

            TestSerializer<IConsumerCredentials, ITwitterCredentials>(twitterCredentials, creds =>
            {
                Assert.Equal(twitterCredentials.ConsumerKey, creds.ConsumerKey);
                Assert.Equal(twitterCredentials.BearerToken, creds.BearerToken);
            });
        }

        [Fact]
        public async Task RateLimits()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var rateLimits = await _tweetinviClient.RateLimits.GetRateLimits(new GetRateLimitsParameters());

            TestSerializer<ICredentialsRateLimits, ICredentialsRateLimits>(rateLimits, deserializedRateLimits =>
            {
                Assert.Equal(rateLimits.ApplicationRateLimitStatusLimit.Remaining, deserializedRateLimits.ApplicationRateLimitStatusLimit.Remaining);
            });
        }

        [Fact]
        public async Task TwitterConfiguration()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var config = await _tweetinviClient.Help.GetTwitterConfiguration();

            TestSerializer<ITwitterConfiguration, ITwitterConfiguration>(config, deserializedConfig =>
            {
                Assert.Equal(config.ShortURLLength, deserializedConfig.ShortURLLength);
            });
        }
    }
}