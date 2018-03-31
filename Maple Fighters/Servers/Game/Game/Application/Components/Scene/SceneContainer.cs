using System.Collections.Generic;
using System.IO;
using System.Threading;
using CommonTools.Log;
using ComponentModel.Common;
using JsonConfig;
using MathematicsHelper;
using Microsoft.Scripting.Hosting;
using Physics.Box2D;
using Physics.Box2D.PhysicsSimulation;
using PythonScripting;
using Game.Common;

namespace Game.Application.Components
{
    public class SceneContainer : Component, ISceneContainer
    {
        private readonly Dictionary<Maps, IGameSceneWrapper> scenes = new Dictionary<Maps, IGameSceneWrapper>();

        private IPythonScriptEngine pythonScriptEngine;
        private ScriptScope scriptScope;

        protected override void OnAwake()
        {
            base.OnAwake();

            pythonScriptEngine = Components.GetComponent<IPythonScriptEngine>().AssertNotNull();

            scriptScope = pythonScriptEngine.GetScriptEngine().CreateScope();
            scriptScope.SetVariable("sceneContainer", this);

            AddScenesViaPython();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            var scenesTemp = new List<IGameSceneWrapper>();
            scenesTemp.AddRange(scenes.Values);

            scenes.Clear();

            foreach (var gameSceneWrapper in scenesTemp)
            {
                gameSceneWrapper.GetScene().Dispose();
            }
        }

        private void AddScenesViaPython()
        {
            var pythonScenesPath = (string)Config.Global.Python.Scenes;
            var pythonScenesScripts = Directory.GetFiles(pythonScenesPath, "*.py", SearchOption.TopDirectoryOnly);

            foreach (var pythonScript in pythonScenesScripts)
            {
                pythonScriptEngine.GetScriptEngine().ExecuteFile(pythonScript, scriptScope);
            }
        }

        public void AddScene(Maps map, Vector2 sceneSize, Vector2 regionSize, PhysicsWorldInfo physicsWorldInfo, bool drawPhysics)
        {
            var gameSceneWrapper = new GameSceneWrapper(map, sceneSize, regionSize);
            scenes.Add(map, gameSceneWrapper);

            var scene = gameSceneWrapper.GetScene();
            scene.Components.AddComponent(new SceneOrderExecutor());
            scene.Components.AddComponent(new PhysicsSimulationExecutor(physicsWorldInfo));
            scene.Components.AddComponent(new PhysicsMapCreator(map));
            scene.Components.AddComponent(new EntityManager());

            gameSceneWrapper.CreateSceneObjectsViaPython();

            if (drawPhysics)
            {
                RunScenePhysicsSimulationWindow(scene.Components, map.ToString(), 800, 600);
            }
        }

        private void RunScenePhysicsSimulationWindow(IContainer sceneContainer, string title, int width, int height)
        {
            var windowCreator = new ThreadStart(() =>
            {
                var physicsSimulationWindow = sceneContainer.AddComponent(new PhysicsSimulationWindow(title, width, height));
                physicsSimulationWindow.Run(PhysicsUtils.UPDATES_PER_SECOND, PhysicsUtils.FRAMES_PER_SECOND);
            });

            var windowThread = new Thread(windowCreator);
            windowThread.Start();
        }

        public IGameSceneWrapper GetSceneWrapper(Maps map)
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