using Game.Application.Components;
using Utilities;

namespace Game.Application.Objects.Components
{
    public class PlayerConfigDataProvider : ComponentBase, IPlayerConfigDataProvider
    {
        private PlayerConfigData configData;

        protected override void OnAwake()
        {
            var yamlConfig = ConfigUtils.LoadYamlConfig(configFile: "player.yml");
            configData = ConfigUtils.ParseConfigData<PlayerConfigData>(yamlConfig);
        }

        public PlayerConfigData Provide()
        {
            return configData;
        }
    }
}