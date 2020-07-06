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
    public class TheDarkForest : IGameScene
    {
        public IMatrixRegion<IGameObject> MatrixRegion { get; }

        public IExposedComponents Components { get; }

        private WorldManager WorldManager { get; }

        private readonly IIdGenerator idGenerator;

        public TheDarkForest(IIdGenerator idGenerator)
        {
            this.idGenerator = idGenerator;

            MatrixRegion = CreateMatrixRegion();
            WorldManager = CreateWorldManager();

            Components = new ComponentsContainer();
            Components.Add(new PlayerSpawnData(new Vector2(-12.8f, -2.95f), new Vector2(10, 5)));
            Components.Add(new GameObjectCollection(GetGameObjects()));
            Components.Add(new GameScenePhysicsExecutor(GetWorldManager()));
        }

        public void Dispose()
        {
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
            var sceneSize = new Vector2(30, 30);
            var regionSize = new Vector2(10, 5);

            return new MatrixRegion<IGameObject>(sceneSize, regionSize);
        }

        IEnumerable<IGameObject> GetGameObjects()
        {
            yield return CreatePortal();
            yield return CreateBlueSnail();
        }

        IGameObject CreatePortal()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(12.5f, -1.125f);
            var portal = new PortalGameObject(id);

            portal.AddProximityChecker(MatrixRegion);
            portal.AddPortalData((byte)Map.Lobby);

            return portal;
        }

        IGameObject CreateBlueSnail()
        {
            var id = idGenerator.GenerateId();
            var position = new Vector2(-2f, -8.2f);
            var blueSnail = new BlueSnailGameObject(id);

            blueSnail.AddProximityChecker(MatrixRegion);

            return blueSnail;
        }
    }
}