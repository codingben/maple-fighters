using System.IO;
using CommonTools.Log;
using Game.InterestManagement;
using MathematicsHelper;
using PythonScripting;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using Shared.Game.Common;
using GameObject = Game.InterestManagement.GameObject;

namespace Game.Application.Components
{
    public class GameSceneWrapper : Component<IServerEntity>
    {
        private readonly Maps map;
        private readonly IScene scene;
        private PythonScriptEngine pythonScriptEngine;

        public GameSceneWrapper(Maps map, IScene scene)
        {
            this.map = map;
            this.scene = scene;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            pythonScriptEngine = Entity.Container.GetComponent<PythonScriptEngine>().AssertNotNull();

            var scope = pythonScriptEngine.GetScriptEngine().CreateScope();
            scope.SetVariable("scene", this);

            AddGameObjectsViaPython();
        }

        private void AddGameObjectsViaPython()
        {
            var pythonScriptsPath = $"../Game/python/scenes/{map}";
            var pythonScripts = Directory.GetFiles(pythonScriptsPath, "*.py", SearchOption.AllDirectories);

            foreach (var pythonScript in pythonScripts)
            {
                pythonScriptEngine.GetScriptEngine().ExecuteFile(pythonScript);
            }
        }

        public void AddGameObject(string name, Vector2 position, Vector2 interestAreaSize)
        {
            var gameObject = new GameObject(name, scene, position, interestAreaSize);
            scene.AddGameObject(gameObject);
        }

        public IScene GetScene()
        {
            return scene;
        }
    }
}