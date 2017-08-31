using System.Collections.Generic;
using CommonTools.Log;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;

namespace Game.InterestManagement
{
    internal class SceneContainer : Component
    {
        private readonly Dictionary<int, IScene> scenes = new Dictionary<int, IScene>();

        public void AddScene(Vector2 sceneSize, Vector2 regionsSize)
        {
            var idGenerator = ServerComponents.Container.GetComponent<IdGenerator>().AssertNotNull() as IdGenerator;
            var sceneId = idGenerator.GenerateId();

            scenes.Add(sceneId, new Scene(sceneId, sceneSize, regionsSize));
        }

        public IScene GetScene(int sceneId)
        {
            if (scenes.TryGetValue(sceneId, out var scene))
            {
                return scene;
            }

            LogUtils.Log($"SceneContainer::GetScene() - Could not find a scene id #{sceneId}", LogMessageType.Error);

            return null;
        }
    }
}