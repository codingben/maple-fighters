using Game.Application.Components;
using Utilities;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Game.Application.Objects.Components
{
    public class NpcConfigDataProvider : ComponentBase, INpcConfigDataProvider
    {
        private NpcConfigData configData;

        protected override void OnAwake()
        {
            var yamlConfig = ConfigUtils.LoadYamlConfig(configFile: "npc.yml");
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
    }
}