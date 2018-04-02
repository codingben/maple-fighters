using System;
using System.Collections.Generic;
using System.Linq;
using Authorization.Service.Application.Components.Interfaces;
using CommonTools.Log;
using ComponentModel.Common;

namespace Authorization.Service.Application.Components
{
    internal class AccessTokensStorage : Component, IAccessTokenCreator, IAccessTokenRemover, IAccessTokenGetter, IAccessTokenExistence
    {
        private readonly object locker = new object();
        private readonly Dictionary<int, string> accessTokens = new Dictionary<int, string>();

        public string Create(int userId)
        {
            lock (locker)
            {
                if (accessTokens.ContainsKey(userId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"A user with id #{userId} already exists in a storage of an access tokens."));
                    return null;
                }

                LogUtils.Log($"Added an access token for the user id #{userId}");

                var accessToken = GenerateAccessToken();
                accessTokens.Add(userId, accessToken);
                return accessToken;
            }

            string GenerateAccessToken() => Guid.NewGuid().ToString("N");
        }

        public void Remove(int userId)
        {
            lock (locker)
            {
                if (!accessTokens.ContainsKey(userId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"A user with id #{userId} does not exist in a storage of an access tokens."));
                    return;
                }

                LogUtils.Log($"Removed an access token for the user id #{userId}");

                accessTokens.Remove(userId);
            }
        }

        public string Get(int userId)
        {
            lock (locker)
            {
                if (accessTokens.TryGetValue(userId, out var accessToken))
                {
                    return accessToken;
                }

                LogUtils.Log(MessageBuilder.Trace($"Could not find an access token for user id #{userId}"));
                return null;
            }
        }

        public int? Get(string accessToken)
        {
            lock (locker)
            {
                if (accessTokens.ContainsValue(accessToken))
                {
                    var userId = accessTokens.FirstOrDefault(x => x.Value == accessToken).Key;
                    return userId;
                }

                LogUtils.Log(MessageBuilder.Trace($"Could not find an user id with provided access token. Access Token: {accessToken}"));
                return null;
            }
        }

        public bool Exists(int userId)
        {
            lock (locker)
            {
                var isExists = accessTokens.Keys.Any(id => id == userId);
                return isExists;
            }
        }

        public bool Exists(string accessToken)
        {
            lock (locker)
            {
                var isExists = accessTokens.Values.Any(token => token == accessToken);
                return isExists;
            }
        }
    }
}