using System.Diagnostics;
using System.Threading;
using Coroutines;
using Physics.Box2D;

namespace Game.Application.Components
{
    public class GameScenePhysicsExecutor : IGameScenePhysicsExecutor
    {
        private readonly IWorldManager worldManager;
        private readonly CoroutineRunner coroutineRunner;
        private readonly CancellationTokenSource cancellationTokenSource;

        public GameScenePhysicsExecutor(IWorldManager worldManager)
        {
            this.worldManager = worldManager;

            coroutineRunner = new CoroutineRunner();
            cancellationTokenSource = new CancellationTokenSource();

            var thread = new Thread(new ParameterizedThreadStart(Execute))
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };
            thread.Start(cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }

        private void Execute(object cancellationToken)
        {
            var timeStep = DefaultSettings.TimeStep;
            var velocityIterations = DefaultSettings.VelocityIterations;
            var positionIterations = DefaultSettings.PositionIterations;
            var sleepTime = DefaultSettings.SleepTime;
            var watch = Stopwatch.StartNew();
            var previousTime = watch.ElapsedMilliseconds / 1000f;
            var elapsed = 0f;
            var token = (CancellationToken)cancellationToken;

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                var currentTime = watch.ElapsedMilliseconds / 1000f;
                elapsed += currentTime - previousTime;
                previousTime = currentTime;

                worldManager.UpdateBodies();

                if (elapsed > timeStep)
                {
                    elapsed -= timeStep;

                    coroutineRunner.Update(timeStep);
                }

                worldManager.Step(timeStep, velocityIterations, positionIterations);

                Thread.Sleep(sleepTime);
            }
        }

        public CoroutineRunner GetCoroutineRunner()
        {
            return coroutineRunner;
        }
    }
}