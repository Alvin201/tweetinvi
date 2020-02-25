﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tweetinvi;
using Tweetinvi.AspNet;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

namespace Examplinvi.AccountActivity.ASP.NETCore
{
    public class Startup
    {
        public static WebhookConfiguration WebhookConfiguration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async Task Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Plugins.Add<WebhooksPlugin>();

            var consumerOnlyCredentials = new ConsumerOnlyCredentials("CONSUMER_TOKEN", "CONSUMER_SECRET")
            {
                BearerToken = "BEARER_TOKEN"
            };

            var client = new TwitterClient(consumerOnlyCredentials);

            if (consumerOnlyCredentials.BearerToken == null)
            {
                await client.Auth.InitializeClientBearerToken();
            }

            WebhookServerInitialization(app, consumerOnlyCredentials);
            RegisterAccountActivities(consumerOnlyCredentials).Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private static void WebhookServerInitialization(IApplicationBuilder app, IConsumerOnlyCredentials consumerOnlyCredentials)
        {
            WebhookConfiguration = new WebhookConfiguration(consumerOnlyCredentials);

            app.UseTweetinviWebhooks(WebhookConfiguration);
        }

        private static async Task RegisterAccountActivities(IConsumerOnlyCredentials consumerOnlyCredentials)
        {
            var client = new TwitterClient(consumerOnlyCredentials);
            var webhookEnvironments = await client.AccountActivity.GetAccountActivityWebhookEnvironments();

            webhookEnvironments.ForEach(environment =>
            {
                var webhookEnvironment = new RegistrableWebhookEnvironment(environment)
                {
                    Credentials = consumerOnlyCredentials
                };

                WebhookConfiguration.AddWebhookEnvironment(webhookEnvironment);

                // If you want your server to be listening to all the already subscribed users
                // Uncomment the line below.

                // await SubscribeToAllAccountActivities(consumerOnlyCredentials, environment);
            });
        }

        private static async Task SubscribeToAllAccountActivities(
            IConsumerOnlyCredentials consumerOnlyCredentials,
            IWebhookEnvironmentDTO environment)
        {
            // If you wish to subscribe to the different account activity events you can do the following
            var subscriptions = await Webhooks.GetListOfSubscriptionsAsync(environment.Name, consumerOnlyCredentials);

            subscriptions.Subscriptions.ForEach(subscription =>
            {
                var activityStream = Stream.CreateAccountActivityStream(subscription.UserId);

                activityStream.JsonObjectReceived += (sender, args) => { Console.WriteLine("json received : " + args.Json); };

                WebhookConfiguration.AddActivityStream(activityStream);
            });
        }
    }
}
