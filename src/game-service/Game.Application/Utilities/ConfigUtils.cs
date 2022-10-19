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
            var user = "codingben";
            var repo = "maple-fighters-configs";
            var branch = Env.GetString("CONFIG_BRANCH");

            if (string.IsNullOrEmpty(branch))
            {
                throw new ArgumentException("CONFIG_BRANCH is not defined");
            }

            return $"https://raw.githubusercontent.com/{user}/{repo}/{branch}/{configFile}";
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