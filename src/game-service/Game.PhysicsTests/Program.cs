using System;
using System.Threading;
using Box2D.Window;
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
            var simulationWindow = new SimulationWindow("Physics Tests", 800, 600);

            simulationWindow.SetView(new CameraView());

            var physicsDrawer = new DrawPhysics(simulationWindow);

            physicsDrawer.AppendFlags(DrawFlags.Aabb);
            physicsDrawer.AppendFlags(DrawFlags.Shape);
            physicsDrawer.AppendFlags(DrawFlags.Pair);
            physicsDrawer.AppendFlags(DrawFlags.Joint);

            simulationWindow.VSync = OpenTK.VSyncMode.On;
            simulationWindow.Run(25.0);
        }
    }
}
