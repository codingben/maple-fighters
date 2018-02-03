using System.IO;
using CommonTools.Log;
using Game.InterestManagement;
using JsonConfig;
using MathematicsHelper;
using Microsoft.Scripting.Hosting;
using PythonScripting;
using ServerApplication.Common.ApplicationBase;
using Shared.Game.Common;

namespace Game.Application.Components
{
    public class GameSceneWrapper : Scene, IGameSceneWrapper
    {
        private readonly Maps map;
        private readonly ScriptScope scriptScope;
        private readonly IPythonScriptEngine pythonScriptEngine;
        private readonly ICharacterSpawnDetailsProvider characterSpawnDetailsProvider;

        public GameSceneWrapper(Maps map, Vector2 sceneSize, Vector2 regionSize)
            : base(sceneSize, regionSize)
        {
            this.map = map;

            pythonScriptEngine = Server.Components.GetComponent<IPythonScriptEngine>().AssertNotNull();
            characterSpawnDetailsProvider = Server.Components.GetComponent<ICharacterSpawnDetailsProvider>().AssertNotNull();

            scriptScope = pythonScriptEngine.GetScriptEngine().CreateScope();
            scriptScope.SetVariable("scene", this);
        }

        public void CreateSceneObjectsViaPython()
        {
            var pythonScenesPath = (string)Config.Global.Python.Scenes;
            var pythonScriptsPath = $"{pythonScenesPath}/{map}";
            var pythonScripts = Directory.GetFiles(pythonScriptsPath, "*.py", SearchOption.AllDirectories);

            foreach (var pythonScript in pythonScripts)
            {
                pythonScriptEngine.GetScriptEngine().ExecuteFile(pythonScript, scriptScope);
            }
        }

        public void CreateSceneObject(ISceneObject sceneObject)
        {
            var createdSceneObject = AddSceneObject(sceneObject);
            var transform = createdSceneObject.Components.GetComponent<ITransform>().AssertNotNull();
            var interestArea = createdSceneObject.Components.AddComponent(new InterestArea(transform.Position, RegionSize));
            interestArea.DetectOverlapsWithRegions();
        }

        public void AddCharacterSpawnDetails(TransformDetails transformDetails) => characterSpawnDetailsProvider.AddCharacterSpawnDetails(map, transformDetails);

        public IScene GetScene() => this;
    }
}