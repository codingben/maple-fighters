using CommonTools.Log;
using ComponentModel.Common;

namespace Game.InterestManagement
{
    public class SceneObject : ISceneObject
    {
        public int Id { get; }
        public string Name { get; }

        public IContainer<ISceneObject> Components { get; }

        public SceneObject(int id, string name, TransformDetails transformDetails)
        {
            Id = id;
            Name = name;

            Components = new Container<ISceneObject>(this);
            Components.AddComponent(new Transform(transformDetails.Position, transformDetails.Size, transformDetails.Direction));
            Components.AddComponent(new PresenceSceneProvider());
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
            var presenceSceneProvider = Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            presenceSceneProvider.Scene?.RemoveSceneObject(Id);

            Components?.Dispose();
        }
    }
}