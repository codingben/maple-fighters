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
            var configEnv = Env.GetString("CONFIG_ENV");

            if (string.IsNullOrEmpty(configEnv))
            {
                throw new ArgumentException("CONFIG_ENV is not defined");
            }

            return $"https://raw.githubusercontent.com/{user}/{repo}/{configEnv}/{configFile}";
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