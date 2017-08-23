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

        public void AddScene(Boundaries boundaries, Vector2 regions)
        {
            var idGenerator = ServerComponents.Container.GetComponent<IdGenerator>().AssertNotNull() as IdGenerator;
            var sceneId = idGenerator.GenerateId();

            scenes.Add(sceneId, new Scene(boundaries, regions));
        }

        public IScene GetScene(int sceneId)
        {
            if (scenes.TryGetValue(sceneId, out var scene))
            {
                return scene;
            }

            LogUtils.Log($"SceneContainer::GetScene() - Could not found a scene id #{sceneId}", LogMessageType.Error);

            return null;
        }
    }
}