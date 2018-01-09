using CommonTools.Log;
using ComponentModel.Common;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components;
using Shared.Game.Common;

namespace Game.InterestManagement
{
    public class SceneObject : ISceneObject
    {
        public int Id { get; }
        public string Name { get; }

        public IContainer<ISceneObject> Container { get; }
        public IScene Scene { get; set; }

        public SceneObject(string name, Vector2 position, float direction)
        {
            Name = name;

            var idGenerator = Server.Entity.Container.GetComponent<IIdGenerator>().AssertNotNull();
            Id = idGenerator.GenerateId();

            Container = new Container<ISceneObject>(this);
            Container.AddComponent(new Transform(position, direction.ToDirections()));
        }

        public virtual void OnAwake()
        {
            // Left blank intentionally
        }

        protected virtual void OnDestroy()
        {
            // Left blank intentionally
        }

        public void Dispose()
        {
            Container?.Dispose();

            OnDestroy();

            Scene?.RemoveSceneObject(Id);
        }
    }
}