using System;
using System.Net;
using DotNetEnv;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Utilities
{
    public class ConfigUtils
    {
        public static string GetYamlConfigUrl(string configFile)
        {
            var configUser = Env.GetString("CONFIG_UESR");
            var configRepo = Env.GetString("CONFIG_REPO");
            var configBranch = Env.GetString("CONFIG_BRANCH");

            if (string.IsNullOrEmpty(configUser))
                throw new ArgumentException("CONFIG_UESR is not defined");

            if (string.IsNullOrEmpty(configRepo))
                throw new ArgumentException("CONFIG_REPO is not defined");

            if (string.IsNullOrEmpty(configBranch))
                throw new ArgumentException("CONFIG_BRANCH is not defined");

            return $"https://raw.githubusercontent.com/{configUser}/{configRepo}/{configBranch}/{configFile}";
        }

        public static string LoadYamlConfig(string url)
        {
            var config = string.Empty;

            using (var client = new WebClient())
            {
                config = client.DownloadString(url);
            }

            return config;
        }

        public static TData ParseConfigData<TData>(string data)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<TData>(data);
        }
    }
}