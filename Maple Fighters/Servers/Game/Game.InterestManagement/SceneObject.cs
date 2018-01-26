using CommonTools.Log;
using ComponentModel.Common;

namespace Game.InterestManagement
{
    public class SceneObject : ISceneObject
    {
        public int Id { get; }
        public string Name { get; }

        public IContainer<ISceneObject> Container { get; }

        public SceneObject(int id, string name, TransformDetails transformDetails)
        {
            Id = id;
            Name = name;

            Container = new Container<ISceneObject>(this);
            Container.AddComponent(new Transform(transformDetails.Position, transformDetails.Size, transformDetails.Direction));
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