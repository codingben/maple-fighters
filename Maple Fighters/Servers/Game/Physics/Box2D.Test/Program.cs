using System;
using System.Threading;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using Physics.Box2D.PhysicsSimulation.Window;

namespace Physics.Box2D.Test
{
    internal class Program
    {
        private static World world;

        private static void Main()
        {
            var openTkWindow = new ThreadStart(() => 
            {
                const string WINDOW_TITLE = "Physics Simulation";
                const int SCREEN_WIDTH = 800;
                const int SCREEN_HEIGHT = 600;

                var game = new PhysicsSimulationWindow(WINDOW_TITLE, SCREEN_WIDTH, SCREEN_HEIGHT);
                game.Load += OnGameLoaded;
                game.Disposed += OnDisposed;

                var physicsDrawer = new DrawPhysics(game);
                physicsDrawer.AppendFlags(DebugDraw.DrawFlags.Aabb);
                physicsDrawer.AppendFlags(DebugDraw.DrawFlags.Shape);

                world = CreateWorld();
                world.SetDebugDraw(physicsDrawer);

                CreateBodies();

                game.Run(60.0, 60.0);
            });
            var openTkThread = new Thread(openTkWindow);
            openTkThread.Start();
        }

        private static void OnGameLoaded(object sender, EventArgs eventArgs)
        {
            var ts = new ThreadStart(() => 
            {
                for (;;)
                {
                    // Prepare for simulation. Typically we use a time step of 1/60 of a
                    // second (60Hz) and 10 iterations. This provides a high quality simulation
                    // in most game scenarios.
                    const float TIME_STEP = 1.0f / 60.0f;
                    const int VELOCITY_ITERATIONS = 8;
                    const int POSITION_ITERATIONS = 3;

                    // Instruct the world to perform a single step of simulation. It is
                    // generally best to keep the time step and iterations fixed.
                    world.Step(TIME_STEP, VELOCITY_ITERATIONS, POSITION_ITERATIONS);

                    Thread.Sleep(10);
                }
            });
            var backgroundThread = new Thread(ts);
            backgroundThread.Start();
        }

        private static void OnDisposed(object sender, EventArgs eventArgs)
        {
            world?.Dispose();
        }

        private static void CreateBodies()
        {
            AddBox(new Vec2(0.0f, 10.0f), new Vec2(5.0f, 5.0f));
            AddBox(new Vec2(0.0f, 20.0f), new Vec2(5.0f, 5.0f));
            var box = AddBox(new Vec2(2.5f, 25.0f), new Vec2(5.0f, 5.0f));
            box.SetLinearVelocity(new Vec2(5, 0));
            box.SetAngularVelocity(5);
            AddBox(new Vec2(-2.5f, 25.0f), new Vec2(5.0f, 5.0f));
            AddStaticBox(new Vec2(0.0f, -10.0f), new Vec2(50.0f, 10.0f));
        }

        private static Body AddBox(Vec2 position, Vec2 size)
        {
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(position.X, position.Y);

            var boxDef = new PolygonDef();
            boxDef.SetAsBox(size.X, size.Y);
            boxDef.Density = 1.0f;
            boxDef.Friction = 0.3f;
            boxDef.Restitution = 0.2f;

            var body = world.CreateBody(bodyDef);
            body.CreateShape(boxDef);
            body.SetMassFromShapes();
            return body;
        }

        private static void AddStaticBox(Vec2 position, Vec2 size)
        {
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(position.X, position.Y);

            var boxDef = new PolygonDef();
            boxDef.SetAsBox(size.X, size.Y);
            boxDef.Density = 0.0f;

            var body = world.CreateBody(bodyDef);
            body.CreateShape(boxDef);
            body.SetMassFromShapes();
        }

        private static World CreateWorld()
        {
            var worldAABB = new AABB();
            worldAABB.LowerBound.Set(-100.0f, -100.0f);
            worldAABB.UpperBound.Set(100.0f, 100.0f);

            const bool DO_SLEEP = true;

            var gravity = new Vec2(0.0f, -9.8f);
            var world = new World(worldAABB, gravity, DO_SLEEP);
            return world;
        }
    }
}