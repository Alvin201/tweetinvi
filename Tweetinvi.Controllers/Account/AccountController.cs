﻿using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Account
{
    public class AccountController : IAccountController
    {
        private readonly IUserQueryExecutor _userQueryExecutor;
        private readonly ITwitterClientFactories _factories;
        private readonly IFactory<IAccountSettings> _accountSettingsUnityFactory;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly ITwitterResultFactory _twitterResultFactory;

        public AccountController(
            IUserQueryExecutor userQueryExecutor,
            IUserFactory userFactory,
            ITwitterClientFactories factories,
            IFactory<IAccountSettings> accountSettingsUnityFactory,
            IJsonObjectConverter jsonObjectConverter,
            ITwitterResultFactory twitterResultFactory)
        {
            _userQueryExecutor = userQueryExecutor;
            _factories = factories;
            _accountSettingsUnityFactory = accountSettingsUnityFactory;
            _jsonObjectConverter = jsonObjectConverter;
            _twitterResultFactory = twitterResultFactory;
        }

        public async Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters, ITwitterRequest request)
        {
            var result = await _userQueryExecutor.GetAuthenticatedUser(parameters, request).ConfigureAwait(false);
            return _twitterResultFactory.Create(result, userDTO => _factories.CreateAuthenticatedUser(userDTO));
        }

        // FOLLOW/UNFOLLOW
        public Task<ITwitterResult<IUserDTO>> FollowUser(IFollowUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.FollowUser(parameters, request);
        }

        public Task<ITwitterResult<IRelationshipDetailsDTO>> UpdateRelationship(IUpdateRelationshipParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.UpdateRelationship(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UnFollowUser(IUnFollowUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.UnFollowUser(parameters, request);
        }

        // FRIENDSHIP

        public Task<ITwitterResult<IRelationshipStateDTO[]>> GetRelationshipsWith(IGetRelationshipsWithParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.GetRelationshipsWith(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendshipIterator(IGetUserIdsRequestingFriendshipParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetUserIdsRequestingFriendshipParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetUserIdsRequestingFriendship(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsYouRequestedToFollowIterator(IGetUserIdsYouRequestedToFollowParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetUserIdsYouRequestedToFollowParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetUserIdsYouRequestedToFollow(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        // BLOCK
        public Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.BlockUser(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.UnblockUser(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.ReportUserForSpam(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIdsIterator(IGetBlockedUserIdsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetBlockedUserIdsParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetBlockedUserIds(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetBlockedUsersIterator(IGetBlockedUsersParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetBlockedUsersParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetBlockedUsers(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        // MUTE
        public Task<ITwitterResult<long[]>> GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.GetUserIdsWhoseRetweetsAreMuted(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetMutedUserIdsIterator(IGetMutedUserIdsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetMutedUserIdsParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetMutedUserIds(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMutedUsersIterator(IGetMutedUsersParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetMutedUsersParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetMutedUsers(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public Task<ITwitterResult<IUserDTO>> MuteUser(IMuteUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.MuteUser(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UnMuteUser(IUnMuteUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.UnMuteUser(parameters, request);
        }

        public IAccountSettings GenerateAccountSettingsFromJson(string json)
        {
            var accountSettingsDTO = _jsonObjectConverter.DeserializeObject<IAccountSettingsDTO>(json);

            if (accountSettingsDTO == null)
            {
                return null;
            }

            return GenerateAccountSettingsFromDTO(accountSettingsDTO);
        }

        private IAccountSettings GenerateAccountSettingsFromDTO(IAccountSettingsDTO accountSettingsDTO)
        {
            if (accountSettingsDTO == null)
            {
                return null;
            }

            var parameterOverride = _accountSettingsUnityFactory.GenerateParameterOverrideWrapper("accountSettingsDTO", accountSettingsDTO);
            return _accountSettingsUnityFactory.Create(parameterOverride);
        }
    }
}