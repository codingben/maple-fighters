using Common.ComponentModel;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class PlayerSpawnDataProvider : ComponentBase
    {
        private PlayerSpawnData playerSpawnData;

        public PlayerSpawnDataProvider(PlayerSpawnData playerSpawnData)
        {
            this.playerSpawnData = playerSpawnData;
        }

        public PlayerSpawnData Provide()
        {
            return playerSpawnData;
        }
    }
}