using Game.Application.Components;
using Utilities;

namespace Game.Application.Objects.Components
{
    public class NpcConfigDataProvider : ComponentBase, INpcConfigDataProvider
    {
        private NpcConfigData configData;

        protected override void OnAwake()
        {
            var yamlConfigUrl = ConfigUtils.GetYamlConfigUrl(configFile: "npc.yml");
            var yamlConfig = ConfigUtils.LoadYamlConfig(url: yamlConfigUrl);
            configData = ConfigUtils.ParseConfigData<NpcConfigData>(yamlConfig);
        }

        public NpcConfigData Provide()
        {
            return configData;
        }
    }
}