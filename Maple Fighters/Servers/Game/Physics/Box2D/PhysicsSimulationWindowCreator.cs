using System;
using System.Threading;
using Box2DX.Dynamics;
using CommonTools.Log;
using ComponentModel.Common;
using Physics.Box2D.PhysicsSimulation;

namespace Physics.Box2D
{
    public class PhysicsSimulationWindowCreator : Component
    {
        private readonly string title;

        private World world;
        private DrawPhysics drawPhysics;
        private PhysicsSimulationWindow physicsSimulationWindow;

        public PhysicsSimulationWindowCreator(string title)
        {
            this.title = title;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            RunPhysicsSimulationWindow();
        }

        private void RunPhysicsSimulationWindow()
        {
            var openTkWindow = new ThreadStart(() =>
            {
                const int SCREEN_WIDTH = 800;
                const int SCREEN_HEIGHT = 600;

                physicsSimulationWindow = new PhysicsSimulationWindow(title, SCREEN_WIDTH, SCREEN_HEIGHT);
                physicsSimulationWindow.Closed += OnPhysicsSimulationWindowClosed;

                drawPhysics = new DrawPhysics(physicsSimulationWindow);
                drawPhysics.AppendFlags(DebugDraw.DrawFlags.Aabb);
                drawPhysics.AppendFlags(DebugDraw.DrawFlags.Shape);

                var physicsWorld = Entity.GetComponent<IPhysicsWorldProvider>().AssertNotNull();
                world = physicsWorld.GetWorld();
                world.SetDebugDraw(drawPhysics);

                const float UPDATES_PER_SECOND = 30.0f;
                const float FRAMES_PER_SECOND = 30.0f;

                physicsSimulationWindow.Run(UPDATES_PER_SECOND, FRAMES_PER_SECOND);
            });
            var openTkThread = new Thread(openTkWindow);
            openTkThread.Start();
        }

        private void OnPhysicsSimulationWindowClosed(object sender, EventArgs eventArgs)
        {
            world.SetDebugDraw(null);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            world.SetDebugDraw(null);
            world = null; // A render frame may be called which will cause an error. This makes sure that it won't happen.

            physicsSimulationWindow.Close();
        }
    }
}