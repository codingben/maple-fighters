using System.Net;
using Game.Application.Components;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Game.Application.Objects.Components
{
    public class PlayerConfigDataProvider : ComponentBase, IPlayerConfigDataProvider
    {
        private PlayerConfigData configData;

        protected override void OnAwake()
        {
            var yamlConfig = LoadYamlConfig();
            configData = ParseConfigData(yamlConfig);
        }

        public PlayerConfigData Provide()
        {
            return configData;
        }

        private PlayerConfigData ParseConfigData(string data)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<PlayerConfigData>(data);
        }

        private string LoadYamlConfig()
        {
            var config = string.Empty;
            var url = "https://raw.githubusercontent.com/codingben/maple-fighters-configs/main/{0}";
            var yamlPath = string.Format(url, "player.yml");

            using (var client = new WebClient())
            {
                config = client.DownloadString(yamlPath);
            }

            return config;
        }
    }
}