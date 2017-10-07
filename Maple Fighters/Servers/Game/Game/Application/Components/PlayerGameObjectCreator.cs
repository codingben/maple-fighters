using CommonTools.Log;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal class PlayerGameObjectCreator : Component<IServerEntity>
    {
        private SceneContainer sceneContainer;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneContainer = Server.Entity.Container.GetComponent<SceneContainer>().AssertNotNull();
        }

        public IGameObject Create(Maps map, Vector2 position)
        {
            var scene = sceneContainer.GetGameSceneWrapper(map).GetScene().AssertNotNull();
            var gameObject = scene.AddGameObject(new InterestManagement.GameObject("Player", scene, position)).AssertNotNull();
            gameObject.Container.AddComponent(new InterestArea(position, scene.RegionSize));
            return gameObject;
        }
    }
}