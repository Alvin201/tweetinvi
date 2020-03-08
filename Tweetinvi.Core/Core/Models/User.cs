﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Models
{
    /// <summary>
    /// Tweetinvi User.
    /// </summary>
    public class User : IUser
    {
        private const string REGEX_PROFILE_IMAGE_SIZE = "_[^\\W_]+(?=(?:\\.[a-zA-Z0-9_]+$))";
        public ITwitterClient Client { get; set; }
        public IUserDTO UserDTO { get; set; }

        public IUserIdentifier UserIdentifier => UserDTO;

        #region Public Attributes

        #region Twitter API Attributes

        // This region represents the information accessible from a Twitter API
        // when querying for a User

        public long? Id
        {
            get => UserDTO?.Id;
            set => throw new InvalidOperationException("Cannot set the Id of a user");
        }

        public string IdStr
        {
            get => UserDTO?.IdStr;
            set => throw new InvalidOperationException("Cannot set the Id of a user");
        }

        public string ScreenName
        {
            get => UserDTO?.ScreenName;
            set => throw new InvalidOperationException("Cannot set the ScreenName of a user");
        }

        public string Name => UserDTO.Name;

        public string Description => UserDTO.Description;

        public ITweetDTO Status => UserDTO.Status;

        public DateTime CreatedAt => UserDTO.CreatedAt;

        public string Location => UserDTO.Location;

        public bool? GeoEnabled => UserDTO.GeoEnabled;

        public string Url => UserDTO.Url;

        public int StatusesCount => UserDTO.StatusesCount;

        public int FollowersCount => UserDTO.FollowersCount;

        public int FriendsCount => UserDTO.FriendsCount;

        public bool? Following => UserDTO.Following;

        public bool Protected => UserDTO.Protected;

        public bool Verified => UserDTO.Verified;

        public IUserEntities Entities => UserDTO.Entities;

        public string ProfileImageUrl => UserDTO.ProfileImageUrl;

        public string ProfileImageUrlFullSize
        {
            get
            {
                var profileImageURL = ProfileImageUrl;
                if (string.IsNullOrEmpty(profileImageURL))
                {
                    return null;
                }

                return Regex.Replace(profileImageURL, REGEX_PROFILE_IMAGE_SIZE, string.Empty);
            }
        }

        public string ProfileImageUrl400x400
        {
            get
            {
                var profileImageURL = ProfileImageUrl;
                if (string.IsNullOrEmpty(profileImageURL))
                {
                    return null;
                }

                return Regex.Replace(profileImageURL, REGEX_PROFILE_IMAGE_SIZE, "_400x400");
            }
        }

        public string ProfileImageUrlHttps => UserDTO.ProfileImageUrlHttps;

        public bool? FollowRequestSent => UserDTO.FollowRequestSent;

        public bool DefaultProfile => UserDTO.DefaultProfile;

        public bool DefaultProfileImage => UserDTO.DefaultProfileImage;

        public int FavouritesCount => UserDTO.FavoritesCount ?? 0;

        public int ListedCount => UserDTO.ListedCount ?? 0;

        public string ProfileSidebarFillColor => UserDTO.ProfileSidebarFillColor;

        public string ProfileSidebarBorderColor => UserDTO.ProfileSidebarBorderColor;

        public bool ProfileBackgroundTile => UserDTO.ProfileBackgroundTile;

        public string ProfileBackgroundColor => UserDTO.ProfileBackgroundColor;

        public string ProfileBackgroundImageUrl => UserDTO.ProfileBackgroundImageUrl;

        public string ProfileBackgroundImageUrlHttps => UserDTO.ProfileBackgroundImageUrlHttps;

        public string ProfileBannerURL => UserDTO.ProfileBannerURL;

        public string ProfileTextColor => UserDTO.ProfileTextColor;

        public string ProfileLinkColor => UserDTO.ProfileLinkColor;

        public bool ProfileUseBackgroundImage => UserDTO.ProfileUseBackgroundImage;

        public bool? IsTranslator => UserDTO.IsTranslator;

        public bool? ContributorsEnabled => UserDTO.ContributorsEnabled;

        public int? UtcOffset => UserDTO.UtcOffset;

        public string TimeZone => UserDTO.TimeZone;

        public IEnumerable<string> WithheldInCountries => UserDTO.WithheldInCountries;

        public string WithheldScope => UserDTO.WithheldScope;

        public bool? Notifications => UserDTO.Notifications;

        #endregion

        #endregion

        public User(IUserDTO userDTO, ITwitterClient client)
        {
            UserDTO = userDTO;
            Client = client;
        }

        // Friends
        public virtual ITwitterIterator<long> GetFriendIds()
        {
            return Client?.Users.GetFriendIds(new GetFriendIdsParameters(this));
        }

        public virtual IMultiLevelCursorIterator<long, IUser> GetFriends()
        {
            return Client?.Users.GetFriends(new GetFriendsParameters(this));
        }

        // Followers
        public virtual ITwitterIterator<long> GetFollowerIds()
        {
            return Client?.Users.GetFollowerIds(new GetFollowerIdsParameters(this));
        }

        public virtual IMultiLevelCursorIterator<long, IUser> GetFollowers()
        {
            return Client?.Users.GetFollowers(new GetFollowersParameters(this));
        }

        // Relationship
        public Task<IRelationshipDetails> GetRelationshipWith(IUserIdentifier user)
        {
            return Client.Users.GetRelationshipBetween(this, user);
        }

        public Task<IRelationshipDetails> GetRelationshipWith(long? userId)
        {
            return Client.Users.GetRelationshipBetween(this, userId);
        }

        public Task<IRelationshipDetails> GetRelationshipWith(string username)
        {
            return Client.Users.GetRelationshipBetween(this, username);
        }

        // Timeline
        public ITwitterIterator<ITweet, long?> GetUserTimelineIterator()
        {
            return Client.Timelines.GetUserTimelineIterator(this);
        }

        // Favorites
        public virtual ITwitterIterator<ITweet, long?> GetFavoriteTweets()
        {
            return Client.Tweets.GetFavoriteTweets(this);
        }

        // Lists
        public ITwitterIterator<ITwitterList> GetListSubscriptions()
        {
            return Client.Lists.GetUserListSubscriptionsIterator(this);
        }

        public ITwitterIterator<ITwitterList> GetOwnedListsIterator()
        {
            return Client.Lists.GetListsOwnedByUserIterator(new GetListsOwnedByAccountByUserParameters(this));
        }

        // Block User
        public virtual Task BlockUser()
        {
            return Client.Users.BlockUser(this);
        }

        public virtual Task UnblockUser()
        {
            return Client.Users.UnblockUser(this);
        }

        // Spam
        public virtual Task ReportUserForSpam()
        {
            return Client.Users.ReportUserForSpam(this);
        }

        // Stream Profile Image
        public Task<Stream> GetProfileImageStream()
        {
            return GetProfileImageStream(ImageSize.Normal);
        }

        public Task<Stream> GetProfileImageStream(ImageSize imageSize)
        {
            return Client.Users.GetProfileImageStream(new GetProfileImageParameters(this)
            {
                ImageSize = imageSize
            });
        }

        public bool Equals(IUser other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id || ScreenName == other.ScreenName;
        }

        public override string ToString()
        {
            return UserDTO?.Name ?? "Undefined";
        }
    }
}