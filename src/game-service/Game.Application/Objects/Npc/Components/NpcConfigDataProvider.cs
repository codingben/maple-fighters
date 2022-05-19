using System.Net;
using Game.Application.Components;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Game.Application.Objects.Components
{
    public class NpcConfigDataProvider : ComponentBase, INpcConfigDataProvider
    {
        private NpcConfigData configData;

        protected override void OnAwake()
        {
            var yamlConfig = LoadYamlConfig();
            configData = ParseConfigData(yamlConfig);
        }

        public NpcConfigData Provide()
        {
            return configData;
        }

        private NpcConfigData ParseConfigData(string data)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<NpcConfigData>(data);
        }

        private string LoadYamlConfig()
        {
            var config = string.Empty;
            var url = "https://raw.githubusercontent.com/benukhanov/maple-fighters-configs/main/{0}";
            var yamlPath = string.Format(url, "npc.yml");

            using (var client = new WebClient())
            {
                config = client.DownloadString(yamlPath);
            }

            return config;
        }
    }
}