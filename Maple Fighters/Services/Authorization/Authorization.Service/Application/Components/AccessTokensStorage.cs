using System;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using ComponentModel.Common;

namespace Authorization.Service.Application.Components
{
    internal class AccessTokensStorage : Component, IAccessTokenCreator, IAccessTokenRemover, IAccessTokenGetter, IAccessTokenExistence
    {
        private readonly object locker = new object();
        private readonly Dictionary<int, string> accessTokens = new Dictionary<int, string>();

        public void Create(int userId)
        {
            lock (locker)
            {
                if (accessTokens.ContainsKey(userId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"A user with id #{userId} already exists in a storage of an access tokens."));
                    return;
                }

                accessTokens.Add(userId, GenerateAccessToken());
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