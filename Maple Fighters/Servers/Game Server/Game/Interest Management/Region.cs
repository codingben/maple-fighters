using System;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Coroutines;
using CommonTools.Log;
using Game.Entities;
using Game.Systems;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components.Coroutines;

namespace Game.InterestManagement
{
    internal class Region : IRegion, IDisposable
    {
        public Vector2 Position { get; }
        public Vector2 Size { get; }

        private readonly Dictionary<int, IEntity> entities = new Dictionary<int, IEntity>();

        private readonly TransformSystem transformSystem;
        private readonly CoroutinesExecuter coroutinesExecuter;

        public Region(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;

            transformSystem = ServerComponents.Container.GetComponent<TransformSystem>().AssertNotNull() as TransformSystem;

            coroutinesExecuter = ServerComponents.Container.GetComponent<CoroutinesExecuter>().AssertNotNull() as CoroutinesExecuter;
            coroutinesExecuter.StartCoroutine(UpdateRegion());
        }

        private IEnumerator<IYieldInstruction> UpdateRegion()
        {
            while (true)
            {
                if (entities.Count == 0)
                {
                    yield return null;
                }

                transformSystem.UpdatePosition(GetAllEntities());

                yield return new WaitForSeconds(0.1f);
            }
        }

        public void AddEntity(IEntity entity)
        {
            if (entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Region::AddEntity() -> An entity with a id #{entity.Id} already exists in a region.", LogMessageType.Warning);
                return;
            }

            entities.Add(entity.Id, entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            if (!entities.ContainsKey(entity.Id))
            {
                LogUtils.Log($"Region::RemoveEntity() -> An entity with a id #{entity.Id} does not exists in a region.", LogMessageType.Warning);
                return;
            }

            entities.Remove(entity.Id);
        }

        public List<IEntity> GetAllEntities()
        {
            return entities.Select(entity => entity.Value).ToList();
        }

        public void Dispose()
        {
            coroutinesExecuter?.Dispose();
        }
    }
}