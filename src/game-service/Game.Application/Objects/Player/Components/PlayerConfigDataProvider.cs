using Game.Application.Components;
using Utilities;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Game.Application.Objects.Components
{
    public class PlayerConfigDataProvider : ComponentBase, IPlayerConfigDataProvider
    {
        private PlayerConfigData configData;

        protected override void OnAwake()
        {
            var yamlConfig = ConfigUtils.LoadYamlConfig(configFile: "player.yml");
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
    }
}