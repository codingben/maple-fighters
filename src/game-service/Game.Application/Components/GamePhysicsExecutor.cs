using System.Collections;
using Common.ComponentModel;
using Physics.Box2D;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GamePhysicsExecutor : ComponentBase
    {
        private readonly IWorldManager worldManager;

        private IGameSceneOrderExecutor gameSceneOrderExecutor;

        public GamePhysicsExecutor(IWorldManager worldManager)
        {
            this.worldManager = worldManager;
        }

        protected override void OnAwake()
        {
            gameSceneOrderExecutor = Components.Get<IGameSceneOrderExecutor>();
            gameSceneOrderExecutor.GetAfterUpdatedRunner().Run(Execute());
        }

        protected override void OnRemoved()
        {
            gameSceneOrderExecutor.GetAfterUpdatedRunner().Stop(Execute());
        }

        private IEnumerator Execute()
        {
            var timeStep = DefaultSettings.TimeStep;
            var velocityIterations = DefaultSettings.VelocityIterations;
            var positionIterations = DefaultSettings.PositionIterations;

            while (true)
            {
                worldManager.Step(timeStep, velocityIterations, positionIterations);

                yield return null;
            }
        }
    }
}