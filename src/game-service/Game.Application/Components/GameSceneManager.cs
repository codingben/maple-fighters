using System.Collections.Generic;
using Common.ComponentModel;
using Common.Components;
using Common.MathematicsHelper;
using Game.Application.Objects;
using InterestManagement;

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

            var lobby = CreateLobby();
            var theDarkForest = CreateTheDarkForest();

            gameSceneCollection.Add(Map.Lobby, lobby);
            gameSceneCollection.Add(Map.TheDarkForest, theDarkForest);
        }

        protected override void OnRemoved()
        {
            gameSceneCollection.Remove(Map.Lobby);
            gameSceneCollection.Remove(Map.TheDarkForest);
        }

        // TODO: Refactor
        private IGameScene CreateLobby()
        {
            var gameScene = new GameScene(new Vector2(40, 5), new Vector2(10, 5));

            // Lobby Spawn Position
            gameScene.GamePlayerSpawnData.SetSpawnPosition(new Vector2(18, -1.86f));

            var region = gameScene.MatrixRegion;

            foreach (var gameObject in CreateLobbyGameObjects(region))
            {
                gameScene.GameObjectCollection.Add(gameObject);
            }

            return gameScene;
        }

        // TODO: Refactor
        private IEnumerable<IGameObject> CreateLobbyGameObjects(IMatrixRegion<IGameObject> region)
        {
            // Guardian Game Object
            var guardianId = idGenerator.GenerateId();
            var guardianPosition = new Vector2(-14.24f, -2.025f);
            var guardian =
                new GuardianGameObject(guardianId, guardianPosition, region);
            guardian.AddBubbleNotification("Hello", 1);

            yield return guardian;

            // Portal Game Object
            var portalId = idGenerator.GenerateId();
            var portalPosition = new Vector2(-17.125f, -1.5f);
            var portal =
                new PortalGameObject(portalId, portalPosition, region);
            portal.AddPortalData((byte)Map.TheDarkForest);

            yield return portal;
        }

        // TODO: Refactor
        private IGameScene CreateTheDarkForest()
        {
            var gameScene = new GameScene(new Vector2(30, 30), new Vector2(10, 5));

            // The Dark Forest Spawn Position
            gameScene.GamePlayerSpawnData.SetSpawnPosition(new Vector2(-12.8f, -2.95f));

            var region = gameScene.MatrixRegion;

            foreach (var gameObject in CreateTheDarkForestGameObjects(region))
            {
                gameScene.GameObjectCollection.Add(gameObject);
            }

            return gameScene;
        }

        // TODO: Refactor
        private IEnumerable<IGameObject> CreateTheDarkForestGameObjects(IMatrixRegion<IGameObject> region)
        {
            // Blue Snail Game Object
            var guardianId = idGenerator.GenerateId();
            var blueSnailPosition = new Vector2(-2f, -8.2f);
            var blueSnail =
                new BlueSnailGameObject(guardianId, blueSnailPosition, region);

            yield return blueSnail;

            // Portal Game Object
            var portalId = idGenerator.GenerateId();
            var portalPosition = new Vector2(12.5f, -1.125f);
            var portal =
                new PortalGameObject(portalId, portalPosition, region);
            portal.AddPortalData((byte)Map.Lobby);

            yield return portal;
        }
    }
}