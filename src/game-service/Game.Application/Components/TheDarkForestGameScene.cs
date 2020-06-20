using Common.Components;
using Common.MathematicsHelper;
using Game.Application.Objects;

namespace Game.Application.Components
{
    public class TheDarkForestGameScene : GameScene
    {
        private readonly IIdGenerator idGenerator;

        public TheDarkForestGameScene(IIdGenerator idGenerator)
            : base(new Vector2(30, 30), new Vector2(10, 5))
        {
            this.idGenerator = idGenerator;

            // TODO: Who will "own" the game objects?
            // TODO: Who will "destroy" game objects?

            CreatePortal();
            CreateBlueSnail();
        }

        void CreatePortal()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(12.5f, -1.125f);
            var scene = this;
            var map = (byte)Map.Lobby;
            var portalGameObject = new PortalGameObject(
                id,
                position,
                scene,
                map);
        }

        void CreateBlueSnail()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(-2f, -8.2f);
            var scene = this;
            var portalGameObject = new BlueSnailGameObject(
                id,
                position,
                scene);
        }
    }
}