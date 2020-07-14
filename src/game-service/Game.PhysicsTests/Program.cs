using System;
using System.Threading;
using Box2D.Window;
using Common.ComponentModel;
using Common.Components;
using Game.Application.Components;
using Game.Application.Objects;
using Game.Application.Objects.Components;
using static Box2DX.Dynamics.DebugDraw;

namespace Game.PhysicsTests
{
    public class Program
    {
        private static void Main(string[] args)
        {
            new Thread(new ThreadStart(RunWindow))
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            }.Start();

            Console.ReadKey();
        }

        private static void RunWindow()
        {
            var simulationWindow = CreateSimulationWindow();
            var physicsDrawer = new DrawPhysics(simulationWindow)
            {
                Flags = DrawFlags.Aabb | DrawFlags.Shape
            };

            var gameScene = CreateMap(Map.TheDarkForest);
            if (gameScene != null)
            {
                gameScene.PhysicsWorldManager.SetDebugDraw(physicsDrawer);

                CreatePlayer(gameScene);
            }

            simulationWindow.Run(25.0);
        }

        private static SimulationWindow CreateSimulationWindow()
        {
            var simulationWindow = new SimulationWindow("Physics Tests", 800, 600);

            simulationWindow.SetView(new CameraView());
            simulationWindow.VSync = OpenTK.VSyncMode.On;

            return simulationWindow;
        }

        private static IGameScene CreateMap(Map map)
        {
            IComponents components = new ComponentsContainer();

            components.Add(new IdGenerator());
            var gameSceneCollection = components.Add(new GameSceneCollection());
            components.Add(new GameSceneManager());

            gameSceneCollection.TryGet(map, out var gameScene);

            return gameScene;
        }

        private static void CreatePlayer(IGameScene gameScene)
        {
            var playerId = 1;
            var player = new PlayerGameObject(playerId);

            player.Components.Add(new MessageSender(null, null));
            player.Components.Add(new PlayerAttackedMessageSender());

            var bodyData = player.CreateBodyData();

            gameScene.GameObjectCollection.Add(player);
            gameScene.PhysicsWorldManager.AddBody(bodyData);
        }
    }
}
