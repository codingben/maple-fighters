using System.Globalization;
using System.IO;
using CommonTools.Log;
using Game.InterestManagement;
using MathematicsHelper;
using Microsoft.Scripting.Hosting;
using Physics.Box2D;
using PythonScripting;
using ServerApplication.Common.ApplicationBase;
using ServiceStack;
using Shared.Game.Common;

namespace Game.Application.Components
{
    public class GameSceneWrapper : Scene, IGameSceneWrapper
    {
        private readonly Maps map;
        private readonly ScriptScope scriptScope;
        private readonly IPythonScriptEngine pythonScriptEngine;
        private readonly ICharacterSpawnPositionDetailsProvider characterSpawnPositionProvider;

        public GameSceneWrapper(Maps map, Vector2 sceneSize, Vector2 regionSize)
            : base(sceneSize, regionSize)
        {
            this.map = map;

            pythonScriptEngine = Server.Entity.Container.GetComponent<IPythonScriptEngine>().AssertNotNull();
            characterSpawnPositionProvider = Server.Entity.Container.GetComponent<ICharacterSpawnPositionDetailsProvider>().AssertNotNull();

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

        public void CreateSceneObject(ISceneObject sceneObject)
        {
            var newSceneObject = AddSceneObject(sceneObject);
            var transform = newSceneObject.Container.GetComponent<ITransform>().AssertNotNull();
            var interestArea = newSceneObject.Container.AddComponent(new InterestArea(transform.Position, RegionSize));
            interestArea.DetectOverlapsWithRegions();
        }

        public void AddCharacterSpawnPosition(Vector2 position, float direction) 
            => characterSpawnPositionProvider.AddSpawnPositionDetails(map, new SpawnPositionDetails(position, direction.ToDirections()));

        public IScene GetScene() => this;
    }
}