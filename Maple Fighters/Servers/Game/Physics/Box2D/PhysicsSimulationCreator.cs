using Box2DX.Dynamics;
using CommonTools.Log;
using ComponentModel.Common;
using Game.InterestManagement;
using Physics.Box2D.PhysicsSimulation;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components;
using ServerCommunicationInterfaces;

namespace Physics.Box2D
{
    public class PhysicsSimulationCreator : Component<ISceneEntity>
    {
        private readonly string windowTitle;
        private readonly DrawPhysics drawPhysics;

        private World world;
        private PhysicsSimulationWindow physicsSimulationWindow;

        public PhysicsSimulationCreator(string title)
        {
            windowTitle = title;

            drawPhysics = new DrawPhysics();
            drawPhysics.AppendFlags(DebugDraw.DrawFlags.Aabb);
            drawPhysics.AppendFlags(DebugDraw.DrawFlags.Shape);
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            var physicsCreator = Entity.Container.GetComponent<IPhysicsWorldCreator>().AssertNotNull();
            world = physicsCreator.GetWorld();
            world.SetDebugDraw(drawPhysics);

            var fiber = Server.Entity.Container.GetComponent<IFiberStarter>().AssertNotNull();
            IExecutionContext fiberExecutor = fiber.GetFiberStarter();

            fiberExecutor.Enqueue(() => 
            {
                const int SCREEN_WIDTH = 1024;
                const int SCREEN_HEIGHT = 768;

                physicsSimulationWindow = new PhysicsSimulationWindow(windowTitle, SCREEN_WIDTH, SCREEN_HEIGHT)
                {
                    World = physicsCreator.GetWorld()
                };
                physicsSimulationWindow.Run(60.0);
            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            physicsSimulationWindow.World?.SetDebugDraw(null);
            physicsSimulationWindow.World = null; // A render frame may be called which will cause an error. This makes sure that it won't happen.
            physicsSimulationWindow.Close();
        }
    }
}