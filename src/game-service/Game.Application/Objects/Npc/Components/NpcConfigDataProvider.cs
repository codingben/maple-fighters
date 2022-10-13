using Game.Application.Components;
using Utilities;

namespace Game.Application.Objects.Components
{
    public class NpcConfigDataProvider : ComponentBase, INpcConfigDataProvider
    {
        private NpcConfigData configData;

        protected override void OnAwake()
        {
            var yamlConfig = ConfigUtils.LoadYamlConfig(configFile: "npc.yml");
            configData = ConfigUtils.ParseConfigData<NpcConfigData>(yamlConfig);
        }

        public NpcConfigData Provide()
        {
            return configData;
        }
    }
}