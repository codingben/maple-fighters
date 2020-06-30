using System.Diagnostics;
using System.Threading;
using Common.ComponentModel;
using Coroutines;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class SceneOrderExecutor : ComponentBase, ISceneOrderExecutor
    {
        private readonly CoroutineRunner beforeUpdateRunner;
        private readonly CoroutineRunner duringUpdateRunner;
        private readonly CoroutineRunner afterUpdatedRunner;
        private readonly Thread thread;
        private readonly CancellationTokenSource cancellationTokenSource;

        public SceneOrderExecutor()
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
            beforeUpdateRunner.StopAll();
            duringUpdateRunner.StopAll();
            afterUpdatedRunner.StopAll();

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();

            // TODO: Investigate thread interruption
            thread.Interrupt();
        }

        private void Execute(object cancellationToken)
        {
            const float UpdateRate = 1f / 30f; // 30 FPS

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

                if (elapsed > UpdateRate)
                {
                    elapsed -= UpdateRate;

                    beforeUpdateRunner.Update(UpdateRate);
                    duringUpdateRunner.Update(UpdateRate);
                    afterUpdatedRunner.Update(UpdateRate);
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