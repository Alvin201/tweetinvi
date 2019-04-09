﻿using System;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Messages;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Json
{
    public static class MessageJson
    {
        [ThreadStatic]
        private static IMessageJsonController _messageJsonController;
        public static IMessageJsonController MessageJsonController
        {
            get
            {
                if (_messageJsonController == null)
                {
                    Initialize();
                }
                
                return _messageJsonController;
            }
        }

        static MessageJson()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _messageJsonController = TweetinviContainer.Resolve<IMessageJsonController>();
        }

        // Get Messages
        public static Task<string> GetLatestMessages(int count)
        {
            return MessageJsonController.GetLatestMessages(count);
        }

        /// <summary>
        /// Warning: Behaviour differs from MessageController.GetLatestMessages.
        /// This method will not make multiple requests to the Twitter API and combine their results,
        /// as that would require parsing the JSON, instead that is left up to the caller.
        /// </summary>
        public static Task<string> GetLatestMessages(IGetMessagesParameters parameters)
        {
            return MessageJsonController.GetLatestMessages(parameters);
        }

        // Publish Message
        public static Task<string> PublishMessage(string text, long targetUserId)
        {
            return MessageJsonController.PublishMessage(text, targetUserId);
        }

        public static Task<string> PublishMessage(IPublishMessageParameters parameters)
        {
            return MessageJsonController.PublishMessage(parameters);
        }

        // Destroy Message
        public static Task<bool> DestroyMessage(IMessage message)
        {
            return MessageJsonController.DestroyMessage(message);
        }

        public static Task<bool> DestroyMessage(IMessageEventDTO messageEventDTO)
        {
            return MessageJsonController.DestroyMessage(messageEventDTO);
        }

        public static Task<bool> DestroyMessage(long messageId)
        {
            return MessageJsonController.DestroyMessage(messageId);
        }
    }
}