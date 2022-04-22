using Game.Application.Components;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Game.Application.Objects.Components
{
    public class NpcConfigDataProvider : ComponentBase, INpcConfigDataProvider
    {
        private NpcConfigData npcConfigData;

        protected override void OnAwake()
        {
            var config = GetConfig();

            npcConfigData = ParseNpcConfigData(config);
        }

        public NpcConfigData Provide()
        {
            return npcConfigData;
        }

        private NpcConfigData ParseNpcConfigData(string data)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<NpcConfigData>(data);
        }

        private string GetConfig()
        {
            return @"
message: 'Hello World!'
messageTime: 5
            ";
        }
    }
}