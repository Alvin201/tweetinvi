﻿using System;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

namespace Examplinvi.NETStandard_2._0
{
    class Program
    {
        static void Main()
        {
            Sync.ExecuteTaskAsync(() =>
            {
                var consumerOnlyCredentials = new ConsumerOnlyCredentials("CONSUMER_TOKEN", "CONSUMER_SECRET")
                {
                    ApplicationOnlyBearerToken = "BEARER_TOKEN"
                };

                IWebhookEnvironmentDTO[] webhookEnvironments = Webhooks.GetAllWebhookEnvironmentsAsync(consumerOnlyCredentials).Result;

                webhookEnvironments.ForEach(env =>
                {
                    Console.WriteLine(env.Name);
                });
            }).Wait();

        }

        // ReSharper disable once UnusedMember.Local
        static void StartServer()
        {
            Plugins.Add<WebhooksPlugin>();

            //var server = Task.Run(() => Examplinvi.WebhooksServer.Program.Main(new string[] { }));

            //var client = TweetinviContainer.Resolve<IWebhookProtocolProcessClient>();

            //client.Start();

            //server.Wait();
        }
    }
}
