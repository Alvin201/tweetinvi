﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    /// <summary>
    /// Access and manage user lists.
    /// </summary>
    public static class TwitterList
    {
        [ThreadStatic]
        private static ITwitterListFactory _twitterListFactory;
        public static ITwitterListFactory TwitterListFactory
        {
            get
            {
                if (_twitterListFactory == null)
                {
                    Initialize();
                }

                return _twitterListFactory;
            }
        }

        [ThreadStatic]
        private static ITwitterListController _twitterListController;
        public static ITwitterListController TwitterListController
        {
            get
            {
                if (_twitterListController == null)
                {
                    Initialize();
                }

                return _twitterListController;
            }
        }

        private static readonly ITwitterListQueryParameterGenerator _twitterListQueryParameterGenerator;
        public static ITwitterListQueryParameterGenerator TwitterListQueryParameterGenerator
        {
            get { return _twitterListQueryParameterGenerator; }
        }


        static TwitterList()
        {
            Initialize();

            _twitterListQueryParameterGenerator = TweetinviContainer.Resolve<ITwitterListQueryParameterGenerator>();
        }

        private static void Initialize()
        {
            _twitterListFactory = TweetinviContainer.Resolve<ITwitterListFactory>();
            _twitterListController = TweetinviContainer.Resolve<ITwitterListController>();
        }

        // Get Existing List

        /// <summary>
        /// Get an existing List
        /// </summary>
        public static Task<ITwitterList> GetExistingList(ITwitterListIdentifier twitterListIdentifier)
        {
            return TwitterListFactory.GetExistingList(twitterListIdentifier);
        }

        /// <summary>
        /// Get an existing List
        /// </summary>
        public static Task<ITwitterList> GetExistingList(long listId)
        {
            return TwitterListFactory.GetExistingList(listId);
        }

        /// <summary>
        /// Get an existing List
        /// </summary>
        public static Task<ITwitterList> GetExistingList(string slug, IUserIdentifier user)
        {
            return TwitterListFactory.GetExistingList(slug, user);
        }

        /// <summary>
        /// Get an existing List
        /// </summary>
        public static Task<ITwitterList> GetExistingList(string slug, long userId)
        {
            return TwitterListFactory.GetExistingList(slug, userId);
        }

        /// <summary>
        /// Get an existing List
        /// </summary>
        public static Task<ITwitterList> GetExistingList(string slug, string userScreenName)
        {
            return TwitterListFactory.GetExistingList(slug, userScreenName);
        }

        // Owner Lists

        /// <summary>
        /// Get the authenticated user's lists
        /// </summary>
        public static Task<IEnumerable<ITwitterList>> GetUserOwnedLists(long userId, int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_OWNED_COUNT)
        {
            return TwitterListController.GetUserOwnedLists(userId, maximumNumberOfListsToRetrieve);
        }

        /// <summary>
        /// Get the authenticated user's lists
        /// </summary>
        public static Task<IEnumerable<ITwitterList>> GetUserOwnedLists(string userScreenName, int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_OWNED_COUNT)
        {
            return TwitterListController.GetUserOwnedLists(userScreenName, maximumNumberOfListsToRetrieve);
        }

        /// <summary>
        /// Get the authenticated user's lists
        /// </summary>
        public static Task<IEnumerable<ITwitterList>> GetUserOwnedLists(IUserIdentifier user, int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_OWNED_COUNT)
        {
            return TwitterListController.GetUserOwnedLists(user, maximumNumberOfListsToRetrieve);
        }

        // Create List

        /// <summary>
        /// Create a list
        /// </summary>
        public static Task<ITwitterList> CreateList(string name, PrivacyMode privacyMode, string description = null)
        {
            return TwitterListFactory.CreateList(name, privacyMode, description);
        }

        // Update List

        /// <summary>
        /// Update a list
        /// </summary>
        public static Task<ITwitterList> UpdateList(ITwitterListIdentifier twitterListIdentifier, ITwitterListUpdateParameters parameters)
        {
            return TwitterListController.UpdateList(twitterListIdentifier, parameters);
        }

        /// <summary>
        /// Update a list
        /// </summary>
        public static Task<ITwitterList> UpdateList(long listId, ITwitterListUpdateParameters parameters)
        {
            return TwitterListController.UpdateList(listId, parameters);
        }

        /// <summary>
        /// Update a list
        /// </summary>
        public static Task<ITwitterList> UpdateList(string slug, IUserIdentifier ownerIdentifier, ITwitterListUpdateParameters parameters)
        {
            return TwitterListController.UpdateList(slug, ownerIdentifier, parameters);
        }

        /// <summary>
        /// Update a list
        /// </summary>
        public static Task<ITwitterList> UpdateList(string slug, long ownerId, ITwitterListUpdateParameters parameters)
        {
            return TwitterListController.UpdateList(slug, ownerId, parameters);
        }

        /// <summary>
        /// Update a list
        /// </summary>
        public static Task<ITwitterList> UpdateList(string slug, string ownerScreenName, ITwitterListUpdateParameters parameters)
        {
            return TwitterListController.UpdateList(slug, ownerScreenName, parameters);
        }

        // Destroy List

        /// <summary>
        /// Destroy a list
        /// </summary>
        public static Task<bool> DestroyList(ITwitterListIdentifier list)
        {
            return TwitterListController.DestroyList(list);
        }

        /// <summary>
        /// Destroy a list
        /// </summary>
        public static Task<bool> DestroyList(long listId)
        {
            return TwitterListController.DestroyList(listId);
        }

        /// <summary>
        /// Destroy a list
        /// </summary>
        public static Task<bool> DestroyList(string slug, IUserIdentifier owner)
        {
            return TwitterListController.DestroyList(slug, owner);
        }

        /// <summary>
        /// Destroy a list
        /// </summary>
        public static Task<bool> DestroyList(string slug, long ownerId)
        {
            return TwitterListController.DestroyList(slug, ownerId);
        }

        /// <summary>
        /// Destroy a list
        /// </summary>
        public static Task<bool> DestroyList(string slug, string ownerScreenName)
        {
            return TwitterListController.DestroyList(slug, ownerScreenName);
        }

        // Get Tweets from List

        /// <summary>
        /// Get tweets displayed in a specific list
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetTweetsFromList(long listId)
        {
            return TwitterListController.GetTweetsFromList(listId);
        }

        /// <summary>
        /// Get tweets displayed in a specific list
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, IUserIdentifier owner)
        {
            return TwitterListController.GetTweetsFromList(slug, owner);
        }

        /// <summary>
        /// Get tweets displayed in a specific list
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, string ownerScreenName)
        {
            return TwitterListController.GetTweetsFromList(slug, ownerScreenName);
        }

        /// <summary>
        /// Get tweets displayed in a specific list
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, long ownerId)
        {
            return TwitterListController.GetTweetsFromList(slug, ownerId);
        }

        /// <summary>
        /// Get tweets displayed in a specific list
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetTweetsFromList(ITwitterListIdentifier list, IGetTweetsFromListParameters parameters = null)
        {
            return TwitterListController.GetTweetsFromList(list, parameters);
        }

        // Get Members of List

        /// <summary>
        /// Get the members of a list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetMembersOfList(ITwitterListIdentifier list, int maxNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListMembers(list, maxNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Get the members of a list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetMembersOfList(long listId, int maxNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListMembers(listId, maxNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Get the members of a list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetMembersOfList(string slug, IUserIdentifier owner, int maxNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListMembers(slug, owner, maxNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Get the members of a list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetMembersOfList(string slug, string ownerScreenName, int maxNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListMembers(slug, ownerScreenName, maxNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Get the members of a list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetMembersOfList(string slug, long ownerId, int maxNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListMembers(slug, ownerId, maxNumberOfUsersToRetrieve);
        }

        // Create Member

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(long listId, long newUserId)
        {
            return TwitterListController.AddMemberToList(listId, newUserId);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(long listId, string newUserName)
        {
            return TwitterListController.AddMemberToList(listId, newUserName);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(long listId, IUserIdentifier newUser)
        {
            return TwitterListController.AddMemberToList(listId, newUser);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(string slug, long ownerId, long newUserId)
        {
            return TwitterListController.AddMemberToList(slug, ownerId, newUserId);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(string slug, long ownerId, string newUserName)
        {
            return TwitterListController.AddMemberToList(slug, ownerId, newUserName);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(string slug, long ownerId, IUserIdentifier newUser)
        {
            return TwitterListController.AddMemberToList(slug, ownerId, newUser);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(string slug, string ownerScreenName, long newUserId)
        {
            return TwitterListController.AddMemberToList(slug, ownerScreenName, newUserId);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(string slug, string ownerScreenName, string newUserName)
        {
            return TwitterListController.AddMemberToList(slug, ownerScreenName, newUserName);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(string slug, string ownerScreenName, IUserIdentifier newUser)
        {
            return TwitterListController.AddMemberToList(slug, ownerScreenName, newUser);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(string slug, IUserIdentifier owner, long newUserId)
        {
            return TwitterListController.AddMemberToList(slug, owner, newUserId);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(string slug, IUserIdentifier owner, string newUserName)
        {
            return TwitterListController.AddMemberToList(slug, owner, newUserName);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(string slug, IUserIdentifier owner, IUserIdentifier newUser)
        {
            return TwitterListController.AddMemberToList(slug, owner, newUser);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(ITwitterListIdentifier list, long newUserId)
        {
            return TwitterListController.AddMemberToList(list, newUserId);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(ITwitterListIdentifier list, string newUserName)
        {
            return TwitterListController.AddMemberToList(list, newUserName);
        }

        /// <summary>
        /// Add a user to become a member of the list
        /// </summary>
        public static Task<bool> AddMemberToList(ITwitterListIdentifier list, IUserIdentifier newUser)
        {
            return TwitterListController.AddMemberToList(list, newUser);
        }

        // Create Multiple Members

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(long listId, IEnumerable<long> newUserIds)
        {
            return TwitterListController.AddMultipleMembersToList(listId, newUserIds);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(long listId, IEnumerable<string> newUserScreenNames)
        {
            return TwitterListController.AddMultipleMembersToList(listId, newUserScreenNames);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(long listId, IEnumerable<IUserIdentifier> newUsers)
        {
            return TwitterListController.AddMultipleMembersToList(listId, newUsers);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(string slug, long ownerId, IEnumerable<long> newUserIds)
        {
            return TwitterListController.AddMultipleMembersToList(slug, ownerId, newUserIds);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(string slug, long ownerId, IEnumerable<string> newUserScreenNames)
        {
            return TwitterListController.AddMultipleMembersToList(slug, ownerId, newUserScreenNames);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(string slug, long ownerId, IEnumerable<IUserIdentifier> newUsers)
        {
            return TwitterListController.AddMultipleMembersToList(slug, ownerId, newUsers);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<long> newUserIds)
        {
            return TwitterListController.AddMultipleMembersToList(slug, ownerScreenName, newUserIds);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<string> newUserScreenNames)
        {
            return TwitterListController.AddMultipleMembersToList(slug, ownerScreenName, newUserScreenNames);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<IUserIdentifier> newUsers)
        {
            return TwitterListController.AddMultipleMembersToList(slug, ownerScreenName, newUsers);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<long> newUserIds)
        {
            return TwitterListController.AddMultipleMembersToList(slug, owner, newUserIds);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<string> newUserScreenNames)
        {
            return TwitterListController.AddMultipleMembersToList(slug, owner, newUserScreenNames);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<IUserIdentifier> newUsers)
        {
            return TwitterListController.AddMultipleMembersToList(slug, owner, newUsers);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<long> newUserIds)
        {
            return TwitterListController.AddMultipleMembersToList(list, newUserIds);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<string> newUserScreenNames)
        {
            return TwitterListController.AddMultipleMembersToList(list, newUserScreenNames);
        }

        /// <summary>
        /// Add multiple users to become members of the list
        /// </summary>
        public static Task<MultiRequestsResult> AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> users)
        {
            return TwitterListController.AddMultipleMembersToList(list, users);
        }


        // Remove Member From List

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(long listId, long userId)
        {
            return TwitterListController.RemoveMemberFromList(listId, userId);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(long listId, string userName)
        {
            return TwitterListController.RemoveMemberFromList(listId, userName);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(long listId, IUserIdentifier user)
        {
            return TwitterListController.RemoveMemberFromList(listId, user);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(string slug, long ownerId, long userId)
        {
            return TwitterListController.RemoveMemberFromList(slug, ownerId, userId);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(string slug, long ownerId, string userName)
        {
            return TwitterListController.RemoveMemberFromList(slug, ownerId, userName);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(string slug, long ownerId, IUserIdentifier user)
        {
            return TwitterListController.RemoveMemberFromList(slug, ownerId, user);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(string slug, string ownerScreenName, long userId)
        {
            return TwitterListController.RemoveMemberFromList(slug, ownerScreenName, userId);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(string slug, string ownerScreenName, string userName)
        {
            return TwitterListController.RemoveMemberFromList(slug, ownerScreenName, userName);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(string slug, string ownerScreenName, IUserIdentifier user)
        {
            return TwitterListController.RemoveMemberFromList(slug, ownerScreenName, user);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(string slug, IUserIdentifier owner, long userId)
        {
            return TwitterListController.RemoveMemberFromList(slug, owner, userId);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(string slug, IUserIdentifier owner, string userName)
        {
            return TwitterListController.RemoveMemberFromList(slug, owner, userName);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(string slug, IUserIdentifier owner, IUserIdentifier user)
        {
            return TwitterListController.RemoveMemberFromList(slug, owner, user);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(ITwitterListIdentifier list, long userId)
        {
            return TwitterListController.RemoveMemberFromList(list, userId);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(ITwitterListIdentifier list, string userName)
        {
            return TwitterListController.RemoveMemberFromList(list, userName);
        }

        /// <summary>
        /// Remove a member from a list
        /// </summary>
        public static Task<bool> RemoveMemberFromList(ITwitterListIdentifier list, IUserIdentifier user)
        {
            return TwitterListController.RemoveMemberFromList(list, user);
        }

        // Remove Multiple

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(long listId, IEnumerable<long> userIdsToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(listId, userIdsToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(long listId, IEnumerable<string> userScreenNamesToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(listId, userScreenNamesToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(long listId, IEnumerable<IUserIdentifier> usersToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(listId, usersToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<long> userIdsToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, ownerId, userIdsToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<string> userScreenNamesToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, ownerId, userScreenNamesToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<IUserIdentifier> usersToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, ownerId, usersToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<long> userIdsToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, ownerScreenName, userIdsToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<string> userScreenNamesToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, ownerScreenName, userScreenNamesToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<IUserIdentifier> usersToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, ownerScreenName, usersToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<long> userIdsToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, owner, userIdsToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<string> userScreenNamesToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, owner, userScreenNamesToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<IUserIdentifier> usersToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, owner, usersToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<long> userIdsToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(list, userIdsToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<string> userScreenNamesToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(list, userScreenNamesToRemove);
        }

        /// <summary>
        /// Remove multiple members from a list
        /// </summary>
        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> usersToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(list, usersToRemove);
        }

        // Get Memberships

        public static Task<IEnumerable<ITwitterList>> GetUserListMemberships(string username, IGetUserListMembershipsParameters parameters = null)
        {
            return TwitterListController.GetUserListsMemberships(new UserIdentifier(username), parameters);
        }

        public static Task<IEnumerable<ITwitterList>> GetUserListMemberships(long userId, IGetUserListMembershipsParameters parameters = null)
        {
            return TwitterListController.GetUserListsMemberships(new UserIdentifier(userId), parameters);
        }

        public static Task<IEnumerable<ITwitterList>> GetUserListMemberships(IUserIdentifier userIdentifier, IGetUserListMembershipsParameters parameters = null)
        {
            return TwitterListController.GetUserListsMemberships(userIdentifier, parameters);
        }

        public static Task<IEnumerable<ITwitterList>> GetUserListMemberships(IGetUserListMembershipsQueryParameters parameters)
        {
            return TwitterListController.GetUserListsMemberships(parameters);
        }

        // Check Membership

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(long listId, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListMember(listId, newUserId);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(long listId, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListMember(listId, newUserName);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(long listId, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListMember(listId, newUser);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(string slug, long ownerId, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, ownerId, newUserId);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(string slug, long ownerId, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, ownerId, newUserName);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(string slug, long ownerId, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, ownerId, newUser);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(string slug, string ownerScreenName, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, ownerScreenName, newUserId);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(string slug, string ownerScreenName, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, ownerScreenName, newUserName);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(string slug, string ownerScreenName, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, ownerScreenName, newUser);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(string slug, IUserIdentifier owner, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, owner, newUserId);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(string slug, IUserIdentifier owner, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, owner, newUserName);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(string slug, IUserIdentifier owner, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, owner, newUser);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(ITwitterListIdentifier list, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListMember(list, newUserId);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(ITwitterListIdentifier list, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListMember(list, newUserName);
        }

        /// <summary>
        /// Check if a user is a member of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            return TwitterListController.CheckIfUserIsAListMember(listIdentifier, user);
        }

        // GET User Subscription List

        /// <summary>
        /// Get the lists the authenticated user has subsribed to
        /// </summary>
        public static Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(long userId, int maxNumberOfListsToRetrieve = TweetinviConsts.LIST_GET_USER_SUBSCRIPTIONS_COUNT)
        {
            return TwitterListController.GetUserSubscribedLists(userId, maxNumberOfListsToRetrieve);
        }

        /// <summary>
        /// Get the lists the authenticated user has subsribed to
        /// </summary>
        public static Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(string userScreenName, int maxNumberOfListsToRetrieve = TweetinviConsts.LIST_GET_USER_SUBSCRIPTIONS_COUNT)
        {
            return TwitterListController.GetUserSubscribedLists(userScreenName, maxNumberOfListsToRetrieve);
        }

        /// <summary>
        /// Get the lists the authenticated user has subsribed to
        /// </summary>
        public static Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(IUserIdentifier user, int maxNumberOfListsToRetrieve = TweetinviConsts.LIST_GET_USER_SUBSCRIPTIONS_COUNT)
        {
            return TwitterListController.GetUserSubscribedLists(user, maxNumberOfListsToRetrieve);
        }

        // Get List Subscribers

        /// <summary>
        /// Get the users who subscribed to a specific list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetListSubscribers(long listId, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(listId, maximumNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Get the users who subscribed to a specific list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetListSubscribers(string slug, IUserIdentifier owner, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(slug, owner, maximumNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Get the users who subscribed to a specific list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetListSubscribers(string slug, string ownerScreenName, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(slug, ownerScreenName, maximumNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Get the users who subscribed to a specific list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetListSubscribers(string slug, long ownerId, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(slug, ownerId, maximumNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Get the users who subscribed to a specific list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetListSubscribers(ITwitterListIdentifier list, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(list, maximumNumberOfUsersToRetrieve);
        }

        // CREATE Subscription

        /// <summary>
        /// Subscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> SubscribeAuthenticatedUserToList(long listId, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(listId);
            }

            return TwitterListController.SubscribeAuthenticatedUserToList(listId);
        }

        /// <summary>
        /// Subscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> SubscribeAuthenticatedUserToList(string slug, IUserIdentifier owner, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(slug, owner);
            }

            return TwitterListController.SubscribeAuthenticatedUserToList(slug, owner);
        }

        /// <summary>
        /// Subscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> SubscribeAuthenticatedUserToList(string slug, string ownerScreenName, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(slug, ownerScreenName);
            }

            return TwitterListController.SubscribeAuthenticatedUserToList(slug, ownerScreenName);
        }

        /// <summary>
        /// Subscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> SubscribeAuthenticatedUserToList(string slug, long ownerId, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(slug, ownerId);
            }

            return TwitterListController.SubscribeAuthenticatedUserToList(slug, ownerId);
        }

        /// <summary>
        /// Subscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> SubscribeAuthenticatedUserToList(ITwitterListIdentifier listIdentifier, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(listIdentifier);
            }

            return TwitterListController.SubscribeAuthenticatedUserToList(listIdentifier);
        }

        // Remove Subscription

        /// <summary>
        /// Unubscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> UnSubscribeAuthenticatedUserToList(long listId, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(listId);
            }

            return TwitterListController.UnSubscribeAuthenticatedUserFromList(listId);
        }

        /// <summary>
        /// Unubscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> UnSubscribeAuthenticatedUserFromList(string slug, IUserIdentifier owner, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(slug, owner);
            }

            return TwitterListController.UnSubscribeAuthenticatedUserFromList(slug, owner);
        }

        /// <summary>
        /// Unubscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> UnSubscribeAuthenticatedUserFromList(string slug, string ownerScreenName, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(slug, ownerScreenName);
            }

            return TwitterListController.UnSubscribeAuthenticatedUserFromList(slug, ownerScreenName);
        }

        /// <summary>
        /// Unubscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> UnSubscribeAuthenticatedUserFromList(string slug, long ownerId, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(slug, ownerId);
            }

            return TwitterListController.UnSubscribeAuthenticatedUserFromList(slug, ownerId);
        }

        /// <summary>
        /// Unubscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier listIdentifier, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(listIdentifier);
            }

            return TwitterListController.UnSubscribeAuthenticatedUserFromList(listIdentifier);
        }

        // Check Subscription

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(long listId, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(listId, newUserId);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(long listId, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(listId, newUserName);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(long listId, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(listId, newUser);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, long ownerId, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerId, newUserId);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, long ownerId, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerId, newUserName);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, long ownerId, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerId, newUser);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerScreenName, newUserId);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerScreenName, newUserName);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerScreenName, newUser);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, owner, newUserId);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, owner, newUserName);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, owner, newUser);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier list, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(list, newUserId);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier list, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListMember(list, newUserName);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(listIdentifier, user);
        }

        // Parameters - Tweets From List

        /// <summary>
        /// Create a parameter to get tweets from a List
        /// </summary>
        public static IGetTweetsFromListParameters CreateTweetsFromListParameters()
        {
            return TwitterListQueryParameterGenerator.CreateTweetsFromListParameters();
        }

        /// <summary>
        /// Create a parameter to update a list
        /// </summary>
        public static ITwitterListUpdateParameters CreateUpdateParameters()
        {
            return TwitterListQueryParameterGenerator.CreateUpdateListParameters();
        }
    }
}