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
        public IScene Scene { get; set; }

        public SceneObject(string name, Vector2 position)
        {
            Name = name;

            var idGenerator = Server.Entity.Container.GetComponent<IdGenerator>().AssertNotNull();
            Id = idGenerator.GenerateId();

            Container = new Container<ISceneObject>(this);
            Container.AddComponent(new Transform(position));
        }

        public void Dispose()
        {
            Container?.Dispose();
        }
    }
}