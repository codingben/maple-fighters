using Common.Components;
using Common.MathematicsHelper;
using Game.Application.Objects;

namespace Game.Application.Components
{
    public class LobbyGameScene : GameScene
    {
        private readonly IIdGenerator idGenerator;

        public LobbyGameScene(IIdGenerator idGenerator)
            : base(new Vector2(40, 5), new Vector2(10, 5))
        {
            this.idGenerator = idGenerator;

            // TODO: Who will "own" the game objects?
            // TODO: Who will "destroy" game objects?

            CreateGuardian();
            CreatePortal();
        }

        void CreateGuardian()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(-14.24f, -2.025f);
            var scene = this;
            var text = "Hello!";
            var time = 5;
            var guardianGameObject = new GuardianGameObject(
                id,
                position,
                scene,
                text,
                time);
        }

        void CreatePortal()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(-17.125f, -1.5f);
            var scene = this;
            var map = (byte)Map.TheDarkForest;
            var portalGameObject = new PortalGameObject(
                id,
                position,
                scene,
                map);
        }
    }
}