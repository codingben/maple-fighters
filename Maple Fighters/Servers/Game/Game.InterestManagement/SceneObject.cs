using CommonTools.Log;
using ComponentModel.Common;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components;

namespace Game.InterestManagement
{
    public class SceneObject : ISceneObject
    {
        public int Id { get; }
        public string Name { get; }

        public IContainer<ISceneObject> Container { get; }

        public SceneObject(string name, Vector2 position, Direction direction)
        {
            Name = name;

            var idGenerator = Server.Entity.GetComponent<IIdGenerator>().AssertNotNull();
            Id = idGenerator.GenerateId();

            Container = new Container<ISceneObject>(this);
            Container.AddComponent(new Transform(position));
            Container.AddComponent(new OrientationProvider(direction));
            Container.AddComponent(new PresenceSceneProvider());
        }

        public virtual void OnAwake()
        {
            // Left blank intentionally
        }

        public virtual void OnDestroy()
        {
            // Left blank intentionally
        }

        public void Dispose()
        {
            var presenceSceneProvider = Container.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            presenceSceneProvider.Scene?.RemoveSceneObject(Id);

            Container?.Dispose();
        }
    }
}