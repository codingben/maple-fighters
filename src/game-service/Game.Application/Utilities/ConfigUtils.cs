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
            var configSource = Env.GetString("CONFIG_SOURCE");

            if (string.IsNullOrEmpty(configSource))
                throw new ArgumentException("CONFIG_SOURCE is not defined");

            return $"https://raw.githubusercontent.com/codingben/maple-fighters-configs/{configSource}/{configFile}";
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