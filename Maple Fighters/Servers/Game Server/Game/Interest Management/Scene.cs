using System.Collections.Generic;
using CommonTools.Log;
using Game.Entities;
using Game.Systems;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal class Scene : IScene
    {
        private readonly int sceneId;
        private IRegion[,] regions;
        private readonly Dictionary<int, IEntity> entities = new Dictionary<int, IEntity>();

        private readonly TransformSystem transformSystem;

        public Scene(int sceneId, Vector2 sceneSize, Vector2 regionsSize)
        {
            this.sceneId = sceneId;

            CreateRegions(sceneSize, regionsSize);

            transformSystem = new TransformSystem();
        }

        private void CreateRegions(Vector2 sceneSize, Vector2 regionsSize)
        {
            var regionsX = (int)(sceneSize.X / regionsSize.X);
            var regionsY = (int)(sceneSize.Y / regionsSize.Y);

            regions = new IRegion[regionsX, regionsY];

            var x = -(sceneSize.X / 2) + regionsSize.X / 2;
            var y = -(sceneSize.Y / 2) + regionsSize.Y / 2;

            for (var i = 0; i < regions.GetLength(0); i++)
            {
                for (var j = 0; j < regions.GetLength(1); j++)
                {
                    var regionPositionX = x + (i * regionsSize.X);
                    var regionPositionY = y + (j * regionsSize.Y);

                    regions[i, j] = new Region(new Rectangle(new Vector2(regionPositionX, regionPositionY),
                        new Vector2(regionsSize.X, regionsSize.Y)));
                }
            }
        }

        public void AddEntity(IEntity entity)
        {
            if (entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Scene::AddEntity() -> An entity with a id #{entity.Id} already exists in a scene.", LogMessageType.Warning);
                return;
            }

            entity.PresenceSceneId = sceneId;

            entities.Add(entity.Id, entity);

            transformSystem.AddEntity(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            if (!entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Scene::AddEntity() -> An entity with a id #{entity.Id} does not exists in a scene.", LogMessageType.Warning);
                return;
            }

            entity.PresenceSceneId = -1;

            entities.Remove(entity.Id);

            transformSystem.RemoveEntity(entity);
        }

        public IEntity GetEntity(int entityId)
        {
            if (entities.TryGetValue(entityId, out var entity))
            {
                return entity;
            }

            LogUtils.Log($"Scene::GetEntity() - Could not find an entity id #{entityId}", LogMessageType.Error);

            return null;
        }

        public IRegion[,] GetAllRegions()
        {
            return regions;
        }
    }
}