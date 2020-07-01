using System.Collections;
using Common.ComponentModel;
using Physics.Box2D;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GamePhysicsExecutor : ComponentBase
    {
        private readonly IWorldWrapper worldWrapper;

        private IGameSceneOrderExecutor gameSceneOrderExecutor;

        public GamePhysicsExecutor(IWorldWrapper worldWrapper)
        {
            this.worldWrapper = worldWrapper;
        }

        protected override void OnAwake()
        {
            gameSceneOrderExecutor = Components.Get<IGameSceneOrderExecutor>();
            gameSceneOrderExecutor.GetAfterUpdatedRunner().Run(Execute());
        }

        protected override void OnRemoved()
        {
            gameSceneOrderExecutor.GetAfterUpdatedRunner().Stop(Execute());

            worldWrapper.Dispose();
        }

        private IEnumerator Execute()
        {
            var world = worldWrapper.GetWorld();
            var timeStep = PhysicsSettings.TimeStep;
            var velocityIterations = PhysicsSettings.VelocityIterations;
            var positionIterations = PhysicsSettings.PositionIterations;

            while (true)
            {
                world.Step(timeStep, velocityIterations, positionIterations);

                yield return null;
            }
        }
    }
}