using CommonTools.Log;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;

namespace Game.InterestManagement
{
    public class GameObject : IGameObject
    {
        public int Id { get; }
        public IScene Scene { get; private set; }
        public IContainer<IGameObject> Container { get; }

        private SceneContainer sceneContainer;

        public GameObject(int sceneId, Vector2 position, Vector2 interestAreaSize)
        {
            var idGenerator = Server.Entity.Container.GetComponent<IdGenerator>().AssertNotNull();
            Id = idGenerator.GenerateId();

            SetScene(sceneId);

            Container = new Container<IGameObject>(this);

            Container.AddComponent(new Transform(position));
            Container.AddComponent(new InterestArea(position, interestAreaSize));
        }

        public GameObject(int sceneId, int id, Vector2 position, Vector2 interestAreaSize)
        {
            Id = id;

            SetScene(sceneId);

            Container = new Container<IGameObject>(this);

            Container.AddComponent(new Transform(position));
            Container.AddComponent(new InterestArea(position, interestAreaSize));
        }

        public void SetScene(int sceneId)
        {
            Scene = GetScene(sceneId);
        }

        private IScene GetScene(int sceneId)
        {
            if (sceneContainer == null)
            {
                sceneContainer = Server.Entity.Container.GetComponent<SceneContainer>().AssertNotNull();
            }

            var scene = sceneContainer.GetScene(sceneId).AssertNotNull();
            return scene;
        }

        public void Dispose()
        {
            Container.RemoveAllComponents();
            Scene.RemoveGameObjectFromScene(Id);
        }
    }
}