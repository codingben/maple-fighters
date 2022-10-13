using Game.Application.Components;
using Utilities;

namespace Game.Application.Objects.Components
{
    public class MobConfigDataProvider : ComponentBase, IMobConfigDataProvider
    {
        private MobConfigData configData;

        protected override void OnAwake()
        {
            var yamlConfig = ConfigUtils.LoadYamlConfig(configFile: "mob.yml");
            configData = ConfigUtils.ParseConfigData<MobConfigData>(yamlConfig);
        }

        public MobConfigData Provide()
        {
            return configData;
        }
    }
}