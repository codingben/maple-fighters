using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using OpenTK;
using Physics.Box2D.PhysicsSimulation;

namespace Physics.Box2D.Test
{
    internal class Program
    {
        private static void Main()
        {
            const int SCREEN_WIDTH = 1024;
            const int SCREEN_HEIGHT = 768;

            var game = new PhysicsSimulationWindow(SCREEN_WIDTH, SCREEN_HEIGHT)
            {
                VSync = VSyncMode.On,
                World = CreateWorld()
            };
            game.Run(60.0);
        }

        private static World CreateWorld()
        {
            // Define the size of the world. Simulation will still work
            // if bodies reach the end of the world, but it will be slower.
            var worldAABB = new AABB();
            worldAABB.LowerBound.Set(-100.0f);
            worldAABB.UpperBound.Set(100.0f);

            // Define the gravity vector.
            var gravity = new Vec2(0.0f, -10.0f);

            // Do we want to let bodies sleep?
            const bool DO_SLEEP = true;

            // Construct a world object, which will hold and simulate the rigid bodies.
            var world = new World(worldAABB, gravity, DO_SLEEP);
            world.SetDebugDraw(new DrawPhysics());

            // Define the ground body.
            var groundBodyDef = new BodyDef();
            groundBodyDef.Position.Set(0.0f, -10.0f);

            // Call the body factory which  creates the ground box shape.
            // The body is also added to the world.
            var groundBody = world.CreateBody(groundBodyDef);

            // Define the ground box shape.
            var groundShapeDef = new PolygonDef();

            // The extents are the half-widths of the box.
            groundShapeDef.SetAsBox(50.0f, 10.0f);

            // Add the ground shape to the ground body.
            groundBody.CreateFixture(groundShapeDef);

            // Define the dynamic body. We set its position and call the body factory.
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(0.0f, 4.0f);
            var body = world.CreateBody(bodyDef);

            // Define another box shape for our dynamic body.
            var shapeDef = new PolygonDef();
            shapeDef.SetAsBox(1.0f, 1.0f);

            // Set the box density to be non-zero, so it will be dynamic.
            shapeDef.Density = 1.0f;

            // Override the default friction.
            shapeDef.Friction = 0.3f;

            // Add the shape to the body.
            body.CreateFixture(shapeDef);

            // Now tell the dynamic body to compute it's mass properties base
            // on its shape.
            body.SetMassFromShapes();
            return world;
        }
    }
}