﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi
{
    public static class JsonSerializer
    {
        private interface IJsonSerializer
        {
            object GetSerializableObject(object source);
            object GetDeserializedObject(string json);
        }

        private class InternalJsonSerializer<T1, T2> : IJsonSerializer
            where T1 : class
            where T2 : class
        {
            private readonly Func<T1, T2> _internalGetSerializableObject;
            private readonly Func<string, T1> _deserializer;

            public InternalJsonSerializer(Func<T1, T2> internalGetSerializableObject, Func<string, T1> deserializer)
            {
                _internalGetSerializableObject = internalGetSerializableObject;
                _deserializer = deserializer;
            }

            public object GetSerializableObject(object source)
            {
                return _internalGetSerializableObject(source as T1);
            }

            public object GetDeserializedObject(string json)
            {
                return _deserializer(json);
            }
        }

        private static readonly Dictionary<Type, IJsonSerializer> _getSerializableObject;

        static JsonSerializer()
        {
            var factories = TweetinviContainer.Resolve<ITwitterClientFactories>();

            _getSerializableObject = new Dictionary<Type, IJsonSerializer>();

            // ReSharper disable RedundantTypeArgumentsOfMethod
            Map<ITweet, ITweetDTO>(u => u.TweetDTO, factories.CreateTweet);
            Map<IUser, IUserDTO>(u => u.UserDTO, factories.CreateUser);
            Map<IMessage, IMessageEventWithAppDTO>(m =>
            {
                var eventWithApp = TweetinviContainer.Resolve<IMessageEventWithAppDTO>();
                eventWithApp.MessageEvent = m.MessageEventDTO;
                eventWithApp.App = m.App;
                return eventWithApp;
            }, factories.CreateMessage);
            Map<ITwitterList, ITwitterListDTO>(l => l.TwitterListDTO, factories.CreateTwitterList);
            Map<ISavedSearch, ISavedSearchDTO>(s => s.SavedSearchDTO, factories.CreateSavedSearch);
            Map<IOEmbedTweet, IOEmbedTweetDTO>(t => t.OembedTweetDTO, factories.CreateOEmbedTweet);
            Map<IRelationshipDetails, IRelationshipDetailsDTO>(r => r.RelationshipDetailsDTO, factories.CreateRelationshipDetails);
            Map<IRelationshipState, IRelationshipStateDTO>(r => r.RelationshipStateDTO, factories.CreateRelationshipState);
            // ReSharper restore RedundantTypeArgumentsOfMethod
        }

        // TO JSON
        public static string ToJson<T>(this T obj) where T : class
        {
            return ToJson(obj, null);
        }

        public static string ToJson<T1, T2>(this T1 obj, Func<T1, T2> getSerializableObject) where T1 : class where T2 : class
        {
            var serializer = new InternalJsonSerializer<T1, T2>(getSerializableObject, null);
            return ToJson(obj, serializer);
        }

        public static string ToJson<T, T1, T2>(this T obj, Func<T1, T2> getSerializableObject) where T1 : class where T2 : class
        {
            var serializer = new InternalJsonSerializer<T1, T2>(getSerializableObject, null);
            return ToJson(obj, serializer);
        }

        private static string ToJson<T>(T obj, IJsonSerializer serializer)
        {
            var type = typeof(T);
            object toSerialize = obj;

            var isGenericType = type.GetTypeInfo().IsGenericType;

            if (obj is IEnumerable enumerable && isGenericType)
            {

                 var genericType = type.GetGenericArguments()[0];

                serializer = serializer ?? GetSerializerFromNonCollectionType(genericType);

                if (serializer != null)
                {
                    var list = new List<object>();

                    foreach (var o in enumerable)
                    {
                        list.Add(serializer.GetSerializableObject(o));
                    }

                    toSerialize = list;
                }
            }

            if (serializer == null)
            {
                serializer = GetSerializerFromNonCollectionType(type);

                if (serializer != null)
                {
                    toSerialize = serializer.GetSerializableObject(obj);
                }
            }

            try
            {
                return JsonConvert.SerializeObject(toSerialize);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "The type provided is probably not compatible with Tweetinvi Json serializer." +
                    "If you think this class should be serializable by default please report on github.com/linvi/tweetinvi.",
                    ex
                );
            }
        }

        // FROM JSON

        public static T ConvertJsonTo<T>(this string json) where T : class
        {
            return ConvertJsonTo<T>(json, null);
        }

        public static T1 ConvertJsonTo<T1, T2>(this string json, Func<string, T2> deserialize) where T1 : class where T2 : class
        {
            var serializer = new InternalJsonSerializer<T2, object>(null, deserialize);
            return ConvertJsonTo<T1>(json, serializer);
        }

        private static T ConvertJsonTo<T>(this string json, IJsonSerializer serializer) where T : class
        {
            var type = typeof(T);

            try
            {
                if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    Type genericType = null;

                    if (type.GetTypeInfo().IsGenericType)
                    {
                        genericType = type.GetGenericArguments()[0];
                    }
                    else if (typeof(Array).IsAssignableFrom(type))
                    {
                        genericType = type.GetElementType();
                    }

                    serializer = serializer ?? GetSerializerFromNonCollectionType(genericType);
                    if (genericType != null && serializer != null)
                    {
                        var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericType));

                        JArray jsonArray = JArray.Parse(json);

                        foreach (var elt in jsonArray)
                        {
                            var eltJson = elt.ToString();
                            list.Add(serializer.GetDeserializedObject(eltJson));
                        }

                        if (typeof(Array).IsAssignableFrom(type))
                        {
                            var array = Array.CreateInstance(genericType, list.Count);
                            list.CopyTo(array, 0);
                            return array as T;
                        }

                        return list as T;
                    }
                }

                serializer = serializer ?? GetSerializerFromNonCollectionType(type);

                if (serializer != null)
                {
                    return serializer.GetDeserializedObject(json) as T;
                }

                return JsonConvert.DeserializeObject<T>(json, JsonPropertiesConverterRepository.Converters);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "The type provided is probably not compatible with Tweetinvi Json serializer." +
                    "If you think this class should be deserializable by default please report on github.com/linvi/tweetinvi.",
                    ex
                );
            }
        }

        private static IJsonSerializer GetSerializerFromNonCollectionType(Type type)
        {
            // Test interfaces
            if (_getSerializableObject.ContainsKey(type))
            {
                return _getSerializableObject[type];
            }

            // Test concrete classes from mapped interfaces
            if (_getSerializableObject.Keys.Any(x => x.IsAssignableFrom(type)))
            {
                return _getSerializableObject.FirstOrDefault(x => x.Key.IsAssignableFrom(type)).Value;
            }

            return null;
        }

        public static void Map<T1, T2>(Func<T1, T2> getSerializableObject, Func<string, T1> deserialize) where T1 : class where T2 : class
        {
            if (_getSerializableObject.ContainsKey(typeof(T1)))
            {
                _getSerializableObject[typeof(T1)] = new InternalJsonSerializer<T1, T2>(getSerializableObject, deserialize);
            }
            else
            {
                _getSerializableObject.Add(typeof(T1), new InternalJsonSerializer<T1, T2>(getSerializableObject, deserialize));
            }
        }
    }
}