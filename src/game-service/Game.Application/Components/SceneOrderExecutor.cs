using System.Threading;
using System.Threading.Tasks;
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

        private CancellationTokenSource source = new CancellationTokenSource();

        public SceneOrderExecutor()
        {
            beforeUpdateRunner = new CoroutineRunner();
            duringUpdateRunner = new CoroutineRunner();
            afterUpdatedRunner = new CoroutineRunner();
        }

        protected override void OnAwake()
        {
            Task.Run(Execute, source.Token);
        }

        protected override void OnRemoved()
        {
            beforeUpdateRunner.StopAll();
            duringUpdateRunner.StopAll();
            afterUpdatedRunner.StopAll();

            source.Cancel(false);
        }

        private async Task Execute()
        {
            while (true)
            {
                beforeUpdateRunner.Update(10);
                duringUpdateRunner.Update(10);
                afterUpdatedRunner.Update(10);

                await Task.Delay(100);
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