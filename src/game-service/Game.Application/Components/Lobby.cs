using System;
using System.Collections.Generic;
using Common.ComponentModel;
using Common.Components;
using Common.MathematicsHelper;
using Game.Application.Objects;
using InterestManagement;
using Physics.Box2D;

namespace Game.Application.Components
{
    public class Lobby : IGameScene
    {
        public IMatrixRegion<IGameObject> MatrixRegion { get; }

        public IExposedComponents Components { get; }

        private WorldManager WorldManager { get; }

        private readonly IIdGenerator idGenerator;
        private readonly IGameObjectCollection gameObjectCollection;

        public Lobby()
        {
            Components = new ComponentsContainer();

            MatrixRegion = CreateMatrixRegion();
            WorldManager = CreateWorldManager();

            idGenerator = Components.Add(new IdGenerator());
            Components.Add(new PlayerSpawnData(new Vector2(18, -1.86f), new Vector2(10, 5)));
            gameObjectCollection = Components.Add(new GameObjectCollection());
            Components.Add(new GamePlayerCreator());
            Components.Add(new GameScenePhysicsExecutor(GetWorldManager()));

            foreach (var gameObject in CreateGameObjects())
            {
                gameObjectCollection.AddGameObject(gameObject);
            }
        }

        public void Dispose()
        {
            // TODO: Dispose all game objects

            ((IDisposable)Components)?.Dispose();
            WorldManager?.Dispose();
        }

        WorldManager CreateWorldManager()
        {
            var lowerBound = new Vector2(-100, -100);
            var upperBound = new Vector2(100, 100);
            var gravity = new Vector2(0, -9.8f);
            var doSleep = false;
            var continuousPhysics = false;

            return new WorldManager(lowerBound, upperBound, gravity, doSleep, continuousPhysics);
        }

        public IBodyManager GetBodyManager()
        {
            return WorldManager;
        }

        public IWorldManager GetWorldManager()
        {
            return WorldManager;
        }

        IMatrixRegion<IGameObject> CreateMatrixRegion()
        {
            var sceneSize = new Vector2(40, 5);
            var regionSize = new Vector2(10, 5);

            return new MatrixRegion<IGameObject>(sceneSize, regionSize);
        }

        IEnumerable<IGameObject> CreateGameObjects()
        {
            yield return CreateGuardian();
            yield return CreatePortal();
        }

        IGameObject CreateGuardian()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(-14.24f, -2.025f);
            var guardian = new GuardianGameObject(id);

            guardian.Transform.SetPosition(position);
            guardian.Transform.SetSize(Vector2.One);
            guardian.AddProximityChecker(MatrixRegion);
            guardian.AddBubbleNotification("Hello", 1);

            return guardian;
        }

        IGameObject CreatePortal()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(-17.125f, -1.5f);
            var portal = new PortalGameObject(id);

            portal.AddProximityChecker(MatrixRegion);
            portal.AddPortalData((byte)Map.TheDarkForest);

            return portal;
        }
    }
}