using System.Collections.Generic;
using System.IO;
using CommonTools.Log;
using ComponentModel.Common;
using Game.InterestManagement;
using MathematicsHelper;
using Microsoft.Scripting.Hosting;
using PythonScripting;
using ServerApplication.Common.ApplicationBase;
using Shared.Game.Common;

namespace Game.Application.Components
{
    public class SceneContainer : Component<IServerEntity>
    {
        private readonly Dictionary<Maps, GameSceneWrapper> scenes = new Dictionary<Maps, GameSceneWrapper>();

        private PythonScriptEngine pythonScriptEngine;
        private ScriptScope scriptScope;

        protected override void OnAwake()
        {
            base.OnAwake();

            pythonScriptEngine = Entity.Container.GetComponent<PythonScriptEngine>().AssertNotNull();

            scriptScope = pythonScriptEngine.GetScriptEngine().CreateScope();
            scriptScope.SetVariable("sceneContainer", this);

            AddScenesViaPython();
        }

        private void AddScenesViaPython()
        {
            const string PYTHON_SCRIPTS_PATH = "../Game/python/scenes";
            var pythonScripts = Directory.GetFiles(PYTHON_SCRIPTS_PATH, "*.py", SearchOption.TopDirectoryOnly);

            foreach (var pythonScript in pythonScripts)
            {
                pythonScriptEngine.GetScriptEngine().ExecuteFile(pythonScript, scriptScope);
            }
        }

        public void AddScene(byte map, Vector2 sceneSize, Vector2 regionSize)
        {
            scenes.Add((Maps)map, new GameSceneWrapper((Maps)map, new Scene(sceneSize, regionSize)));
        }

        public GameSceneWrapper GetGameSceneWrapper(Maps map)
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