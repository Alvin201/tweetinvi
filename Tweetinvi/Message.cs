﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    /// <summary>
    /// Receive and send private messages between users.
    /// </summary>
    public static class Message
    {
        [ThreadStatic]
        private static IMessageFactory _messageFactory;

        /// <summary>
        /// Factory used to create Messages
        /// </summary>
        public static IMessageFactory MessageFactory
        {
            get
            {
                if (_messageFactory == null)
                {
                    Initialize();
                }

                return _messageFactory;
            }
        }

        [ThreadStatic]
        private static IMessageController _messageController;

        /// <summary>
        /// Controller handling any Message request
        /// </summary>
        public static IMessageController MessageController
        {
            get
            {
                if (_messageController == null)
                {
                    Initialize();
                }

                return _messageController;
            }
        }

        static Message()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _messageFactory = TweetinviContainer.Resolve<IMessageFactory>();
            _messageController = TweetinviContainer.Resolve<IMessageController>();
        }

        // Factory

        /// <summary>
        /// Get an existing message from its id
        /// </summary>
        public static Task<IMessage> GetExistingMessage(long messageId)
        {
            return MessageFactory.GetExistingMessage(messageId);
        }

        // Controller

        /// <summary>
        /// Get the latest messages sent or received
        /// </summary>
        public static async Task<IEnumerable<IMessage>> GetLatestMessages(int count = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            var asyncOperation = await MessageController.GetLatestMessages(count);

            return asyncOperation.Result;
        }


        /// <summary>
        /// Get the latest messages sent or received
        /// </summary>
        public static Task<AsyncCursorResult<IEnumerable<IMessage>>> GetLatestMessagesWithCursor(int count = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            return MessageController.GetLatestMessages(count);
        }

        /// <summary>
        /// Get the latest messages sent or received
        /// </summary>
        public static async Task<IEnumerable<IMessage>> GetLatestMessages(IGetMessagesParameters queryParameters)
        {
            var latestMessages = await MessageController.GetLatestMessages(queryParameters);

            return latestMessages.Result;
        }

        /// <summary>
        /// Get the latest messages sent or received
        /// </summary>
        public static Task<AsyncCursorResult<IEnumerable<IMessage>>> GetLatestMessagesWithCursor(IGetMessagesParameters queryParameters)
        {
            return MessageController.GetLatestMessages(queryParameters);
        }
    }
}