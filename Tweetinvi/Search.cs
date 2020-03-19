﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    /// <summary>
    /// Search Tweets and Users.
    /// </summary>
    public static class Search
    {
        [ThreadStatic]
        private static ISearchController _searchController;

        /// <summary>
        /// Controller handling any Search request
        /// </summary>
        public static ISearchController SearchController
        {
            get
            {
                if (_searchController == null)
                {
                    Initialize();
                }

                return _searchController;
            }
        }

        public static ISearchQueryParameterGenerator SearchQueryParameterGenerator { get; set; }

        static Search()
        {
            Initialize();

            SearchQueryParameterGenerator = TweetinviContainer.Resolve<ISearchQueryParameterGenerator>();
        }

        private static void Initialize()
        {
            _searchController = TweetinviContainer.Resolve<ISearchController>();
        }

        /// <summary>
        /// Search direct replies to a specific tweet
        /// </summary>
        public static Task<IEnumerable<ITweet>> SearchDirectRepliesTo(ITweet tweet)
        {
            return SearchController.SearchDirectRepliesTo(tweet);
        }

        /// <summary>
        /// Search replies to a tweet and recursively to the replies' replies
        /// </summary>
        public static Task<IEnumerable<ITweet>> SearchRepliesTo(ITweet tweet, bool recursiveReplies)
        {
            return SearchController.SearchRepliesTo(tweet, recursiveReplies);
        }

        /// <summary>
        /// Create a parameter to search tweets for a specific query
        /// </summary>
        public static ISearchTweetsParameters CreateTweetSearchParameter(string query)
        {
            return SearchQueryParameterGenerator.CreateSearchTweetParameter(query);
        }

        /// <summary>
        /// Create a parameter to search tweets for a specific GeoCode
        /// </summary>
        public static ISearchTweetsParameters CreateTweetSearchParameter(IGeoCode geoCode)
        {
            return SearchQueryParameterGenerator.CreateSearchTweetParameter(geoCode);
        }

        /// <summary>
        /// Create a parameter to search tweets for some specific coordinates and radius
        /// </summary>
        public static ISearchTweetsParameters CreateTweetSearchParameter(ICoordinates coordinates, int radius, DistanceMeasure measure)
        {
            return SearchQueryParameterGenerator.CreateSearchTweetParameter(coordinates, radius, measure);
        }

        /// <summary>
        /// Create a parameter to search tweets for some specific coordinates and radius
        /// </summary>
        public static ISearchTweetsParameters CreateTweetSearchParameter(double latitude, double longitude, int radius, DistanceMeasure measure)
        {
            return SearchQueryParameterGenerator.CreateSearchTweetParameter(latitude, longitude, radius, measure);
        }

        // USER

        /// <summary>
        /// Create a parameter to search users from a query
        /// </summary>
        public static ISearchUsersParameters CreateUserSearchParameter(string query)
        {
            return SearchQueryParameterGenerator.CreateUserSearchParameters(query);
        }

        /// <summary>
        /// Create a parameter to search users from a query at a specific page of the result indexation
        /// </summary>
        public static Task<IEnumerable<IUser>> SearchUsers(string query, int maximumNumberOfResults = 20, int page = 0)
        {
            var searchParameters = CreateUserSearchParameter(query);
            searchParameters.Page = page;
            searchParameters.MaximumNumberOfResults = maximumNumberOfResults;

            return SearchController.SearchUsers(searchParameters);
        }
    }
}