using System.Diagnostics;
using System.Threading;
using Common.ComponentModel;
using Coroutines;
using Physics.Box2D;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class GameSceneOrderExecutor : ComponentBase, IGameSceneOrderExecutor
    {
        private readonly CoroutineRunner beforeUpdateRunner;
        private readonly CoroutineRunner duringUpdateRunner;
        private readonly CoroutineRunner afterUpdatedRunner;

        private readonly Thread thread;
        private readonly CancellationTokenSource cancellationTokenSource;

        public GameSceneOrderExecutor()
        {
            beforeUpdateRunner = new CoroutineRunner();
            duringUpdateRunner = new CoroutineRunner();
            afterUpdatedRunner = new CoroutineRunner();

            thread = new Thread(new ParameterizedThreadStart(Execute))
            {
                Priority = ThreadPriority.Lowest
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

                if (elapsed > timeStep)
                {
                    elapsed -= timeStep;

                    beforeUpdateRunner.Update(timeStep);
                    duringUpdateRunner.Update(timeStep);
                    afterUpdatedRunner.Update(timeStep);
                }
            }
        }

        public CoroutineRunner GetBeforeUpdateRunner()
        {
            return beforeUpdateRunner;
        }

        public CoroutineRunner GetDuringUpdateRunner()
        {
            return duringUpdateRunner;
        }

        public CoroutineRunner GetAfterUpdatedRunner()
        {
            return afterUpdatedRunner;
        }
    }
}