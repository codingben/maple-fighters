using System;
using System.Diagnostics;
using System.Threading;
using Coroutines;

namespace Game.Physics
{
    public class PhysicsExecutor : IPhysicsExecutor
    {
        private readonly Action updateBodies;
        private readonly Action simulatePhysics;
        private readonly CoroutineRunner coroutineRunner;
        private readonly CancellationTokenSource cancellationTokenSource;

        public PhysicsExecutor(Action updateBodies = null, Action simulatePhysics = null)
        {
            this.updateBodies = updateBodies;
            this.simulatePhysics = simulatePhysics;

            coroutineRunner = new CoroutineRunner();
            cancellationTokenSource = new CancellationTokenSource();

            StartExecution();
        }

        public void Dispose()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }

        private void StartExecution()
        {
            var thread = new Thread(new ParameterizedThreadStart(Execute));
            thread.Start(cancellationTokenSource.Token);
        }

        private void Execute(object cancellationToken)
        {
            var timeStep = PhysicsSettings.TimeStep;
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

                updateBodies?.Invoke();

                if (elapsed > timeStep)
                {
                    elapsed -= timeStep;

                    coroutineRunner.Update(timeStep);
                }

                simulatePhysics?.Invoke();

                Thread.Sleep(10);
            }
        }

        public ICoroutineRunner GetCoroutineRunner()
        {
            return coroutineRunner;
        }
    }
}