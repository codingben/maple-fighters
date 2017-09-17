using System.Collections.Generic;
using CommonTools.Log;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using Shared.Game.Common;

namespace Game.InterestManagement
{
    public class SceneContainer : Component<IServerEntity>
    {
        private readonly Dictionary<Maps, IScene> scenes = new Dictionary<Maps, IScene>();

        public void AddScene(Maps map, Vector2 sceneSize, Vector2 regionsSize)
        {
            scenes.Add(map, new Scene(sceneSize, regionsSize));
        }

        public IScene GetScene(Maps map)
        {
            if (scenes.TryGetValue(map, out var scene))
            {
                return scene;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a scene with map {map}"), LogMessageType.Error);
            return null;
        }
    }
}