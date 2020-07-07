using Common.ComponentModel;
using Common.Components;
using Common.MathematicsHelper;
using Game.Application.Objects;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameSceneManager : ComponentBase
    {
        private IIdGenerator idGenerator;
        private IGameSceneCollection gameSceneCollection;

        protected override void OnAwake()
        {
            idGenerator = Components.Get<IIdGenerator>();
            gameSceneCollection = Components.Get<IGameSceneCollection>();

            var lobby = CreateMap(Map.Lobby);
            var theDarkForest = CreateMap(Map.TheDarkForest);

            gameSceneCollection.Add(Map.Lobby, lobby);
            gameSceneCollection.Add(Map.TheDarkForest, theDarkForest);
        }

        protected override void OnRemoved()
        {
            gameSceneCollection.Remove(Map.Lobby);
            gameSceneCollection.Remove(Map.TheDarkForest);
        }

        // TODO: Move out
        private IGameScene CreateMap(Map map)
        {
            IGameScene gameScene = null;

            switch (map)
            {
                case Map.Lobby:
                {
                    gameScene = new GameScene(new Vector2(40, 5), new Vector2(40, 5));

                    // Guardian Game Object
                    var guardianId = idGenerator.GenerateId();
                    var guardian = new GuardianGameObject(guardianId, new Vector2(-14.24f, -2.025f));
                    guardian.AddProximityChecker(gameScene.MatrixRegion);
                    guardian.AddBubbleNotification("Hello", 1);

                    gameScene.GameObjectCollection.Add(guardian);

                    // Portal Game Object
                    var portalId = idGenerator.GenerateId();
                    var portal = new PortalGameObject(portalId, new Vector2(-17.125f, -1.5f));
                    portal.AddProximityChecker(gameScene.MatrixRegion);
                    portal.AddPortalData((byte)Map.TheDarkForest);

                    gameScene.GameObjectCollection.Add(portal);
                    break;
                }

                case Map.TheDarkForest:
                {
                    gameScene = new GameScene(new Vector2(30, 30), new Vector2(40, 5));

                    // Blue Snail Game Object
                    var guardianId = idGenerator.GenerateId();
                    var blueSnail = new BlueSnailGameObject(guardianId, new Vector2(-2f, -8.2f));
                    blueSnail.AddProximityChecker(gameScene.MatrixRegion);

                    gameScene.GameObjectCollection.Add(blueSnail);

                    // Portal Game Object
                    var portalId = idGenerator.GenerateId();
                    var portal = new PortalGameObject(portalId, new Vector2(12.5f, -1.125f));
                    portal.AddProximityChecker(gameScene.MatrixRegion);
                    portal.AddPortalData((byte)Map.Lobby);

                    gameScene.GameObjectCollection.Add(portal);
                    break;
                }
            }

            return gameScene;
        }
    }
}