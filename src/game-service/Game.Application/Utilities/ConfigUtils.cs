using System;
using System.Net;
using DotNetEnv;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Utilities
{
    public class ConfigUtils
    {
        public static string LoadYamlConfig(string configFile)
        {
            var config = string.Empty;
            var configEnv = Env.GetString("CONFIG_ENV");
            var url = "https://raw.githubusercontent.com/codingben/maple-fighters-configs/{0}/{1}";
            var yamlPath = string.Format(url, configEnv, configFile);

            if (string.IsNullOrEmpty(configEnv))
            {
                throw new ArgumentException("CONFIG_ENV is not defined");
            }

            using (var client = new WebClient())
            {
                config = client.DownloadString(yamlPath);
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