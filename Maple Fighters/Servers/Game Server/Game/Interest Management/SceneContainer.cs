using System.Collections.Generic;
using CommonTools.Log;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;

namespace Game.InterestManagement
{
    internal class SceneContainer : IComponent
    {
        private readonly Dictionary<int, IScene> scenes = new Dictionary<int, IScene>();

        public void AddScene(Boundaries boundaries, int regions)
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

        public void Dispose()
        {
            // Left blank intentionally
        }
    }
}