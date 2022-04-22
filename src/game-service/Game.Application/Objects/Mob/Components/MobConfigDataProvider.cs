using Game.Application.Components;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Game.Application.Objects.Components
{
    public class MobConfigDataProvider : ComponentBase, IMobConfigDataProvider
    {
        private MobConfigData mobConfigData;

        protected override void OnAwake()
        {
            var config = GetConfig();

            mobConfigData = ParseMobConfigData(config);
        }

        public MobConfigData Provide()
        {
            return mobConfigData;
        }

        private MobConfigData ParseMobConfigData(string data)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<MobConfigData>(data);
        }

        private string GetConfig()
        {
            return @"
bodyWidth: 0.3625
bodyHeight: 0.825
bodyDensity: 0.0
speed: 0.75
distance: 2.5
            ";
        }
    }
}