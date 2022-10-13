using Game.Application.Components;
using Utilities;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Game.Application.Objects.Components
{
    public class MobConfigDataProvider : ComponentBase, IMobConfigDataProvider
    {
        private MobConfigData configData;

        protected override void OnAwake()
        {
            var yamlConfig = ConfigUtils.LoadYamlConfig(configFile: "mob.yml");
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
    }
}