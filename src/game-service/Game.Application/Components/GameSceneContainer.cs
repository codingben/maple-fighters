using System.Collections.Generic;
using Common.ComponentModel;
using Common.Components;
using Common.MathematicsHelper;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameSceneContainer : ComponentBase, IGameSceneContainer
    {
        private readonly IReadOnlyDictionary<Map, IGameScene> container;

        public GameSceneContainer()
        {
            var lobbyScene = new GameScene(worldSize: new Vector2(40, 5), regionSize: new Vector2(10, 5));
            lobbyScene.PlayerSpawnData = new PlayerSpawnData(Vector2.Zero, Vector2.Zero);

            var theDarkForestScene = new GameScene(worldSize: new Vector2(30, 30), regionSize: new Vector2(10, 5));
            theDarkForestScene.PlayerSpawnData = new PlayerSpawnData(Vector2.Zero, Vector2.Zero);

            // TODO: Consider adding new scenes using method or OnAwake()
            container = new Dictionary<Map, IGameScene>()
            {
                { Map.Lobby, lobbyScene },
                { Map.TheDarkForest, theDarkForestScene }
            };
        }

        protected override void OnAwake()
        {
            CreatePortalToTheDarkForest();

            // TODO: Remove this from here
            void CreatePortalToTheDarkForest()
            {
                var idGenerator = Components.Get<IIdGenerator>();
                var id = idGenerator.GenerateId();
                var portalGameObject = new GameObject(id, "Portal");
                var scene = container[0]; // Lobby Scene

                portalGameObject.Transform.SetPosition(new Vector2(-17.125f, -1.5f));
                portalGameObject.Transform.SetSize(Vector2.One);

                portalGameObject.Components.Add(new PresenceSceneProvider(scene));
                portalGameObject.Components.Add(new ProximityChecker());
                portalGameObject.Components.Add(new PortalData((byte)Map.TheDarkForest));
            }
        }

        protected override void OnRemoved()
        {
            foreach (var scene in container.Values)
            {
                scene?.Dispose();
            }
        }

        public bool TryGetScene(Map map, out IGameScene scene)
        {
            return container.TryGetValue(map, out scene);
        }
    }
}