using System.Collections.Generic;
using CommonTools.Log;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;

namespace Game.InterestManagement
{
    internal class SceneContainer : Component
    {
        private readonly Dictionary<int, IScene> scenes = new Dictionary<int, IScene>();

        public void AddScene(int sceneId, Vector2 sceneSize, Vector2 regionsSize)
        {
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