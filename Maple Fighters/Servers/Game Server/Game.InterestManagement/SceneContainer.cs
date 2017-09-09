using System.Collections.Generic;
using CommonTools.Log;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;

namespace Game.InterestManagement
{
    public class SceneContainer : Component<IServerEntity>
    {
        private readonly Dictionary<int, IScene> scenes = new Dictionary<int, IScene>();

        public void AddScene(int sceneId, Vector2 sceneSize, Vector2 regionsSize)
        {
            scenes.Add(sceneId, new Scene(sceneSize, regionsSize));
        }

        public IScene GetScene(int sceneId)
        {
            if (scenes.TryGetValue(sceneId, out var scene))
            {
                return scene;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a scene with id #{sceneId}"), LogMessageType.Error);
            return null;
        }
    }
}