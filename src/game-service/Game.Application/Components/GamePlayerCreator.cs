using Common.ComponentModel;
using Common.Components;
using Game.Application.Objects;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GamePlayerCreator : ComponentBase, IGamePlayerCreator
    {
        private IIdGenerator idGenerator;
        private IPlayerSpawnData playerSpawnData;
        private IGameObjectCollection gameObjectCollection;

        protected override void OnAwake()
        {
            idGenerator = Components.Get<IIdGenerator>();
            playerSpawnData = Components.Get<IPlayerSpawnData>();
        }

        public IGameObject Create()
        {
            var id = idGenerator.GenerateId();
            var player = new PlayerGameObject(id);

            player.Transform.SetPosition(playerSpawnData.GetPosition());
            player.Transform.SetSize(playerSpawnData.GetSize());

            gameObjectCollection.Add(player);

            return player;
        }
    }
}