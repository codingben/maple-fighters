using System.Collections.Generic;
using Common.ComponentModel;
using Common.Components;
using Common.MathematicsHelper;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameSceneCollection : ComponentBase, IGameSceneCollection
    {
        private readonly IDictionary<Map, IGameScene> collection;

        public GameSceneCollection()
        {
            collection = new Dictionary<Map, IGameScene>();
        }

        protected override void OnAwake()
        {
            CreateLobbyGameObjects();
            CreateTheDarkForestGameObjects();

            // TODO: Remove this from here
            void CreateLobbyGameObjects()
            {
                CreatePortalToTheDarkForest();
                CreateGuardianForLobby();
            }

            // TODO: Remove this from here
            void CreateTheDarkForestGameObjects()
            {
                CreateMobForTheDarkForest();
                CreatePortalToLobby();
            }

            // TODO: Remove this from here
            void CreatePortalToTheDarkForest()
            {
                var idGenerator = Components.Get<IIdGenerator>();
                var id = idGenerator.GenerateId();
                var portalGameObject = new GameObject(id, "Portal");
                var scene = container[Map.Lobby];

                portalGameObject.Transform.SetPosition(new Vector2(-17.125f, -1.5f));
                portalGameObject.Transform.SetSize(Vector2.One);

                portalGameObject.Components.Add(new PresenceSceneProvider(scene));
                portalGameObject.Components.Add(new ProximityChecker());
                portalGameObject.Components.Add(new PortalData((byte)Map.TheDarkForest));
            }

            // TODO: Remove this from here
            void CreateGuardianForLobby()
            {
                var idGenerator = Components.Get<IIdGenerator>();
                var id = idGenerator.GenerateId();
                var portalGameObject = new GameObject(id, "Guardian");
                var scene = container[Map.Lobby];

                portalGameObject.Transform.SetPosition(new Vector2(-14.24f, -2.025f));
                portalGameObject.Transform.SetSize(Vector2.One);

                portalGameObject.Components.Add(new PresenceSceneProvider(scene));
                portalGameObject.Components.Add(new ProximityChecker());
                portalGameObject.Components.Add(new BubbleNotificationSender("Hello!", 1));
            }

            // TODO: Remove this from here
            void CreateMobForTheDarkForest()
            {
                var idGenerator = Components.Get<IIdGenerator>();
                var id = idGenerator.GenerateId();
                var portalGameObject = new GameObject(id, "BlueSnail");
                var scene = container[Map.TheDarkForest];

                portalGameObject.Transform.SetPosition(new Vector2(-2f, -8.2f));
                portalGameObject.Transform.SetSize(Vector2.One);

                portalGameObject.Components.Add(new PresenceSceneProvider(scene));
                portalGameObject.Components.Add(new ProximityChecker());
            }

            // TODO: Remove this from here
            void CreatePortalToLobby()
            {
                var idGenerator = Components.Get<IIdGenerator>();
                var id = idGenerator.GenerateId();
                var portalGameObject = new GameObject(id, "Portal");
                var scene = container[Map.TheDarkForest];

                portalGameObject.Transform.SetPosition(new Vector2(12.5f, -1.125f));
                portalGameObject.Transform.SetSize(Vector2.One);

                portalGameObject.Components.Add(new PresenceSceneProvider(scene));
                portalGameObject.Components.Add(new ProximityChecker());
                portalGameObject.Components.Add(new PortalData((byte)Map.Lobby));
            }
        }

        public void AddScene(Map map, IGameScene scene)
        {
            collection.Add(map, scene);
        }

        public void RemoveScene(Map map)
        {
            collection.Remove(map);
        }

        public bool TryGetScene(Map map, out IGameScene scene)
        {
            return collection.TryGetValue(map, out scene);
        }
    }
}