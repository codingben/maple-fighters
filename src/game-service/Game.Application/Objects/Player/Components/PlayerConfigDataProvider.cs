using Game.Application.Components;
using Utilities;

namespace Game.Application.Objects.Components
{
    public class PlayerConfigDataProvider : ComponentBase, IPlayerConfigDataProvider
    {
        private PlayerConfigData configData;

        protected override void OnAwake()
        {
            var yamlConfigUrl = ConfigUtils.GetYamlConfigUrl(configFile: "player.yml");
            var yamlConfig = ConfigUtils.LoadYamlConfig(url: yamlConfigUrl);
            configData = ConfigUtils.ParseConfigData<PlayerConfigData>(yamlConfig);
        }

        public PlayerConfigData Provide()
        {
            return configData;
        }
    }
}