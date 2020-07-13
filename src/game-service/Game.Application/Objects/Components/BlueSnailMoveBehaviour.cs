using System;
using System.Collections;
using Common.ComponentModel;
using Common.MathematicsHelper;
using Coroutines;
using Physics.Box2D;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class BlueSnailMoveBehaviour : ComponentBase
    {
        private IGameObject blueSnail;
        private IPhysicsWorldManager physicsWorldManager;
        private ICoroutineRunner coroutineRunner;
        private BodyData bodyData;

        protected override void OnAwake()
        {
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            blueSnail = gameObjectGetter.Get();

            var presenceMapProvider = Components.Get<IPresenceMapProvider>();
            var gameScene = presenceMapProvider.GetMap();
            physicsWorldManager = gameScene.PhysicsWorldManager;
            coroutineRunner = gameScene.PhysicsExecutor.GetCoroutineRunner();
            coroutineRunner.Run(Move());
        }

        protected override void OnRemoved()
        {
            coroutineRunner.Stop(Move());
        }

        private IEnumerator Move()
        {
            var id = blueSnail.Id;
            var position = blueSnail.Transform.Position;
            var direction = 0.1f;
            var speed = 1;
            var distance = 10;

            physicsWorldManager.GetBody(id, out bodyData);

            while (true)
            {
                position += new Vector2(direction, 0) * speed;

                if (Math.Abs(position.X) > distance)
                {
                    direction *= -1;
                }

                if (bodyData.Body != null)
                {
                    var body = bodyData.Body;
                    body.SetXForm(position.FromVector2(), body.GetAngle());
                }

                yield return null;
            }
        }
    }
}