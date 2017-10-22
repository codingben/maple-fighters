using System.IO;
using CommonTools.Log;
using Game.InterestManagement;
using Microsoft.Scripting.Hosting;
using PythonScripting;
using ServerApplication.Common.ApplicationBase;
using Shared.Game.Common;

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

        public void AddSceneObject(ISceneObject sceneObject)
        {
            var newSceneObject = scene.AddSceneObject(sceneObject);

            var transform = newSceneObject.Container.GetComponent<ITransform>().AssertNotNull();
            var interestArea = newSceneObject.Container.AddComponent(new InterestArea(transform.Position, scene.RegionSize));
            interestArea.DetectOverlapsWithRegions();
        }

        public IScene GetScene()
        {
            return scene;
        }
    }
}