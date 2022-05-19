using System.Net;
using Game.Application.Components;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Game.Application.Objects.Components
{
    public class MobConfigDataProvider : ComponentBase, IMobConfigDataProvider
    {
        private MobConfigData configData;

        protected override void OnAwake()
        {
            var yamlConfig = LoadYamlConfig();
            configData = ParseConfigData(yamlConfig);
        }

        public MobConfigData Provide()
        {
            return configData;
        }

        private MobConfigData ParseConfigData(string data)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<MobConfigData>(data);
        }

        private string LoadYamlConfig()
        {
            var config = string.Empty;
            var url = "https://raw.githubusercontent.com/benukhanov/maple-fighters-configs/main/{0}";
            var yamlPath = string.Format(url, "mob.yml");

            using (var client = new WebClient())
            {
                config = client.DownloadString(yamlPath);
            }

            return config;
        }
    }
}