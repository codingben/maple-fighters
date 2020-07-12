using System;
using System.Threading;
using Box2D.Window;
using Common.ComponentModel;
using Common.Components;
using Game.Application.Components;
using static Box2DX.Dynamics.DebugDraw;

namespace Game.PhysicsTests
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var thread = new Thread(new ThreadStart(CreateWindow))
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };

            thread.Start();

            Console.ReadKey();
        }

        private static void CreateWindow()
        {
            IComponents components = new ComponentsContainer();
            components.Add(new IdGenerator());
            components.Add(new GameSceneCollection());
            components.Add(new GameSceneManager());

            var simulationWindow = new SimulationWindow("Physics Tests", 800, 600);
            simulationWindow.SetView(new CameraView());

            var physicsDrawer = new DrawPhysics(simulationWindow);
            physicsDrawer.AppendFlags(DrawFlags.Aabb);
            physicsDrawer.AppendFlags(DrawFlags.Shape);
            physicsDrawer.AppendFlags(DrawFlags.Pair);
            physicsDrawer.AppendFlags(DrawFlags.Joint);

            var gameSceneCollection = components.Get<IGameSceneCollection>();
            if (gameSceneCollection.TryGet(Map.TheDarkForest, out var gameScene))
            {
                gameScene.PhysicsWorldManager.SetDebugDraw(physicsDrawer);
            }

            simulationWindow.VSync = OpenTK.VSyncMode.On;
            simulationWindow.Run(25.0);
        }
    }
}
