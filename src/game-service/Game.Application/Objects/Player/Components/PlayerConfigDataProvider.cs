using Game.Application.Components;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Game.Application.Objects.Components
{
    public class PlayerConfigDataProvider : ComponentBase, IPlayerConfigDataProvider
    {
        private PlayerConfigData playerConfigData;

        protected override void OnAwake()
        {
            var config = GetConfig();

            playerConfigData = ParsePlayerConfigData(config);
        }

        public PlayerConfigData Provide()
        {
            return playerConfigData;
        }

        private PlayerConfigData ParsePlayerConfigData(string data)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<PlayerConfigData>(data);
        }

        private string GetConfig()
        {
            return @"
bodyWidth: 0.3625
bodyHeight: 0.825
bodyDensity: 0.1
            ";
        }
    }
}