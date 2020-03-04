using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class TwitterListsRequester : BaseRequester, ITwitterListsRequester
    {
        private readonly ITwitterResultFactory _twitterResultFactory;
        private readonly ITwitterClientFactories _factories;
        private readonly ITwitterListController _twitterListController;
        private readonly ITwitterListsClientRequiredParametersValidator _validator;

        public TwitterListsRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            ITwitterResultFactory twitterResultFactory,
            ITwitterClientFactories factories,
            ITwitterListController twitterListController,
            ITwitterListsClientRequiredParametersValidator validator)
            : base(client, clientEvents)
        {
            _twitterResultFactory = twitterResultFactory;
            _factories = factories;
            _twitterListController = twitterListController;
            _validator = validator;
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> CreateList(ICreateListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.CreateList(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> GetList(IGetListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.GetList(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO[], ITwitterList[]>> GetListsSubscribedByUser(IGetListsSubscribedByUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.GetListsSubscribedByUser(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dtos => dtos.Select(dto => _factories.CreateTwitterList(dto)).ToArray());
            });
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> UpdateList(IUpdateListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.UpdateList(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO>> DestroyList(IDestroyListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _twitterListController.DestroyList(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _twitterListController.GetListsOwnedByUserIterator(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> AddMemberToList(IAddMemberToListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.AddMemberToList(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> AddMembersToList(IAddMembersToListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.AddMembersToList(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListMembershipsIterator(IGetUserListMembershipsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _twitterListController.GetUserListMembershipsIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfListIterator(IGetMembersOfListParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _twitterListController.GetMembersOfListIterator(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> CheckIfUserIsAListMember(ICheckIfUserIsMemberOfListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.CheckIfUserIsAListMember(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> RemoveMemberFromList(IRemoveMemberFromListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.RemoveMemberFromList(parameters, request);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> RemoveMembersFromList(IRemoveMembersFromListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.RemoveMembersFromList(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> SubscribeToList(ISubscribeToListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.SubscribeToList(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> UnsubscribeFromList(IUnsubscribeFromListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.UnsubscribeFromList(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetListSubscribersIterator(IGetListSubscribersParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _twitterListController.GetListSubscribers(parameters, request);

        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListSubscriptionsIterator(IGetUserListSubscriptionsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _twitterListController.GetUserListSubscriptions(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> CheckIfUserIsSubscriberOfList(ICheckIfUserIsSubscriberOfListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.CheckIfUserIsSubscriberOfList(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetTweetsFromListIterator(IGetTweetsFromListParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            return _twitterListController.GetTweetsFromListIterator(parameters, request);
        }
    }
}