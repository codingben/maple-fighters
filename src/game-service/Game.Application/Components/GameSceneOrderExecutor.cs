using System.Diagnostics;
using System.Threading;
using Common.ComponentModel;
using Coroutines;
using Physics.Box2D;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameSceneOrderExecutor : ComponentBase, IGameSceneOrderExecutor
    {
        private readonly IWorldManager worldManager;
        private readonly CoroutineRunner coroutineRunner;
        private readonly Thread thread;
        private readonly CancellationTokenSource cancellationTokenSource;

        public GameSceneOrderExecutor(IWorldManager worldManager)
        {
            this.worldManager = worldManager;

            coroutineRunner = new CoroutineRunner();

            thread = new Thread(new ParameterizedThreadStart(Execute))
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };

            cancellationTokenSource = new CancellationTokenSource();
        }

        protected override void OnAwake()
        {
            thread.Start(cancellationTokenSource.Token);
        }

        protected override void OnRemoved()
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

                worldManager.Update();

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