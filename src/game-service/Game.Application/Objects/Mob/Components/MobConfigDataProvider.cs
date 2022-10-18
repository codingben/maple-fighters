using Game.Application.Components;
using Utilities;

namespace Game.Application.Objects.Components
{
    public class MobConfigDataProvider : ComponentBase, IMobConfigDataProvider
    {
        private MobConfigData configData;

        protected override void OnAwake()
        {
            var yamlConfigUrl = ConfigUtils.GetYamlConfigUrl(configFile: "mob.yml");
            var yamlConfig = ConfigUtils.LoadYamlConfig(url: yamlConfigUrl);
            configData = ConfigUtils.ParseConfigData<MobConfigData>(yamlConfig);
        }

        public MobConfigData Provide()
        {
            return configData;
        }
    }
}