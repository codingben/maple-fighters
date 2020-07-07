using System;
using System.Collections;
using System.Threading;
using Box2D.Window;
using Box2DX.Dynamics;
using Common.ComponentModel;
using Common.Components;
using Common.MathematicsHelper;
using Game.Application.Components;
using Physics.Box2D;
using static Box2DX.Dynamics.DebugDraw;

namespace Game.Tests
{
    public class EntityManager : IDisposable
    {
        private readonly IGameScenePhysicsExecutor gameScenePhysicsExecutor;
        private readonly IBodyManager bodyManager;

        private BodyData bodyData;

        public EntityManager(IGameScenePhysicsExecutor gameScenePhysicsExecutor, IBodyManager bodyManager)
        {
            this.gameScenePhysicsExecutor = gameScenePhysicsExecutor;
            this.bodyManager = bodyManager;

            gameScenePhysicsExecutor.GetCoroutineRunner().Run(SimulateWorld());
            gameScenePhysicsExecutor.GetCoroutineRunner().Run(MovePlayer());
        }

        public void Dispose()
        {
            gameScenePhysicsExecutor.GetCoroutineRunner().Stop(SimulateWorld());
            gameScenePhysicsExecutor.GetCoroutineRunner().Stop(MovePlayer());
        }

        private void AddBox()
        {
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(0, 30);

            var polygonDef = new PolygonDef();
            polygonDef.SetAsBox(5, 5);
            polygonDef.Density = 0.0f;
            polygonDef.Filter = new FilterData();

            var newBodyData = new NewBodyData(2, bodyDef, polygonDef);

            bodyManager.AddBody(newBodyData);
        }

        private void RemoveBox()
        {
            bodyManager.RemoveBody(2);
        }

        private IEnumerator SimulateWorld()
        {
            yield return CountTo(5, 1.5f);
        }

        IEnumerator CountTo(int num, float delay)
        {
            for (int i = 1; i <= num; ++i)
            {
                yield return delay;

                AddBox();

                Console.WriteLine("Add");

                yield return null;

                bodyManager.GetBody(2, out bodyData);

                yield return delay;

                RemoveBox();

                Console.WriteLine("Remove");
            }
        }

        private IEnumerator MovePlayer()
        {
            Console.WriteLine("MovePlayer()");

            var position = new Vector2(0, 30);
            var direction = 0.1f;
            var speed = 1;
            var distance = 10;

            while (true)
            {
                position += new Vector2(direction, 0) * speed;

                if (Math.Abs(position.X) > distance)
                {
                    direction *= -1;
                }

                if (bodyData.Body != null)
                {
                    bodyData.Body.SetXForm(position.FromVector2(), bodyData.Body.GetAngle());
                }

                yield return null;
            }
        }
    }

    public class Box : IDisposable
    {
        private IGameScenePhysicsExecutor gameScenePhysicsExecutor;
        private Body body;

        public Box(IGameScenePhysicsExecutor gameScenePhysicsExecutor, Body body)
        {
            this.gameScenePhysicsExecutor = gameScenePhysicsExecutor;
            this.body = body;

            gameScenePhysicsExecutor.GetCoroutineRunner().Run(AddOrRemovePlayer());
            gameScenePhysicsExecutor.GetCoroutineRunner().Run(MovePlayer());
        }

        public void Dispose()
        {
            gameScenePhysicsExecutor.GetCoroutineRunner().Stop(AddOrRemovePlayer());
            gameScenePhysicsExecutor.GetCoroutineRunner().Stop(MovePlayer());
        }

        private IEnumerator AddOrRemovePlayer()
        {
            Console.WriteLine("AddOrRemovePlayer()");
            yield return null;
        }

        private IEnumerator MovePlayer()
        {
            Console.WriteLine("MovePlayer()");

            var position = new Vector2(0, 15);
            var direction = 0.1f;
            var speed = 1;
            var distance = 10;

            while (true)
            {
                position += new Vector2(direction, 0) * speed;

                if (Math.Abs(position.X) > distance)
                {
                    direction *= -1;
                }

                body.SetXForm(position.FromVector2(), body.GetAngle());

                yield return null;
            }
        }

        private IEnumerator SimulateWorld()
        {
            yield return CountTo(5, 1f);
        }

        IEnumerator CountTo(int num, float delay)
        {
            for (int i = 1; i <= num; ++i)
            {
                yield return delay;

                Console.WriteLine("SimulateWorld()");
            }
        }
    }

    class Program
    {
        private static IWorldManager worldManager;

        private static void Main()
        {
            var lobby = new Lobby();

            var bodyDef = new BodyDef();
            bodyDef.Position.Set(0, 0);

            var polygonDef = new PolygonDef();
            polygonDef.SetAsBox(50, 5);
            polygonDef.Density = 0.0f;
            polygonDef.Filter = new FilterData();

            var bodyDef1 = new BodyDef();
            bodyDef1.Position.Set(0, 15);

            var polygonDef1 = new PolygonDef();
            polygonDef1.SetAsBox(5, 5);
            polygonDef1.Density = 0.0f;
            polygonDef1.Filter = new FilterData();

            var newBodyData = new NewBodyData(0, bodyDef, polygonDef);
            var newBodyData1 = new NewBodyData(1, bodyDef1, polygonDef1);

            lobby.GetBodyManager().AddBody(newBodyData);
            lobby.GetBodyManager().AddBody(newBodyData1);

            worldManager = lobby.GetWorldManager();

            Thread.Sleep(1000);

            lobby.GetBodyManager().GetBody(1, out var bodyData);

            var gameScenePhysicsExecutor = lobby.Components.Get<IGameScenePhysicsExecutor>();
            var blueSnail = new Box(gameScenePhysicsExecutor, bodyData.Body);
            var entityManager = new EntityManager(gameScenePhysicsExecutor, lobby.GetBodyManager());

            var windowThread = new Thread(new ThreadStart(() =>
            {
                var game = new SimulationWindow("Physics Simulation", 800, 600);
                game.SetView(new CameraView());

                var physicsDrawer = new DrawPhysics(game);

                physicsDrawer.AppendFlags(DrawFlags.Aabb);
                physicsDrawer.AppendFlags(DrawFlags.Shape);
                physicsDrawer.AppendFlags(DrawFlags.Pair);
                physicsDrawer.AppendFlags(DrawFlags.Joint);

                var worldManager = lobby.GetWorldManager();
                worldManager.SetDebugDraw(physicsDrawer);

                game.VSync = OpenTK.VSyncMode.On;
                game.Run(25.0);
            }));
            windowThread.IsBackground = true;
            windowThread.Priority = ThreadPriority.Lowest;
            windowThread.Start();

            Console.ReadKey();

            lobby.Dispose();
            blueSnail.Dispose();
            entityManager.Dispose();

            Console.ReadKey();
        }
    }
}