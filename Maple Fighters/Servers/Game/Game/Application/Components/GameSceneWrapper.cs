using System.IO;
using CommonTools.Log;
using Game.InterestManagement;
using MathematicsHelper;
using Microsoft.Scripting.Hosting;
using PythonScripting;
using ServerApplication.Common.ApplicationBase;
using Shared.Game.Common;
using SceneObject = Game.InterestManagement.SceneObject;

namespace Game.Application.Components
{
    public class GameSceneWrapper : IGameSceneWrapper
    {
        private readonly Maps map;
        private readonly IScene scene;

        private readonly IPythonScriptEngine pythonScriptEngine;
        private readonly ScriptScope scriptScope;

        public GameSceneWrapper(Maps map, IScene scene)
        {
            this.map = map;
            this.scene = scene;

            pythonScriptEngine = Server.Entity.Container.GetComponent<IPythonScriptEngine>().AssertNotNull();

            scriptScope = pythonScriptEngine.GetScriptEngine().CreateScope();
            scriptScope.SetVariable("scene", this);

            AddSceneObjectsViaPython();
        }

        private void AddSceneObjectsViaPython()
        {
            var pythonScriptsPath = $"python/scenes/{map}";
            var pythonScripts = Directory.GetFiles(pythonScriptsPath, "*.py", SearchOption.AllDirectories);

            foreach (var pythonScript in pythonScripts)
            {
                pythonScriptEngine.GetScriptEngine().ExecuteFile(pythonScript, scriptScope);
            }
        }

        public void AddSceneObject(string name, Vector2 position)
        {
            var sceneObject = new SceneObject(name, position);
            scene.AddSceneObject(sceneObject);

            var interestArea = sceneObject.Container.AddComponent(new InterestArea(position, scene.RegionSize));
            interestArea.DetectOverlapsWithRegions();
        }

        public IScene GetScene()
        {
            return scene;
        }
    }
}