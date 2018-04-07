using System.IO;
using CommonTools.Log;
using Game.Application.Components.Interfaces;
using JsonConfig;
using MathematicsHelper;
using Microsoft.Scripting.Hosting;
using PythonScripting;
using ServerApplication.Common.ApplicationBase;
using Game.Common;
using InterestManagement;
using InterestManagement.Components;
using InterestManagement.Components.Interfaces;

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

            pythonScriptEngine = ServerComponents.GetComponent<IPythonScriptEngine>().AssertNotNull();
            characterSpawnDetailsProvider = ServerComponents.GetComponent<ICharacterSpawnDetailsProvider>().AssertNotNull();

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
            var positionTransform = createdSceneObject.Components.GetComponent<IPositionTransform>().AssertNotNull();
            var interestArea = createdSceneObject.Components.AddComponent(new InterestArea(positionTransform.Position, RegionSize));
            interestArea.DetectOverlapsWithRegions();
        }

        public void AddCharacterSpawnDetails(TransformDetails transformDetails) => characterSpawnDetailsProvider.AddCharacterSpawnDetails(map, transformDetails);

        public IScene GetScene() => this;
    }
}