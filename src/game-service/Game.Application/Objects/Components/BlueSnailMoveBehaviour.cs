using System;
using System.Collections;
using Common.ComponentModel;
using Common.MathematicsHelper;
using Game.Application.Components;
using Physics.Box2D;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class BlueSnailMoveBehaviour : ComponentBase
    {
        private IGameObject blueSnail;
        private IGameScene gameScene;
        private BodyData blueSnailBodyData;

        protected override void OnAwake()
        {
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            blueSnail = gameObjectGetter.Get();

            var presenceMapProvider = Components.Get<IPresenceMapProvider>();
            gameScene = presenceMapProvider.GetMap();
            gameScene.PhysicsExecutor.GetCoroutineRunner().Run(Move());
        }

        protected override void OnRemoved()
        {
            gameScene.PhysicsExecutor.GetCoroutineRunner().Stop(Move());
        }

        private IEnumerator Move()
        {
            var transform = blueSnail.Transform;
            var position = transform.Position;
            var direction = 0.1f;
            var speed = 1;
            var distance = 10;

            var id = blueSnail.Id;
            gameScene.PhysicsWorldManager.GetBody(id, out blueSnailBodyData);

            while (true)
            {
                position += new Vector2(direction, 0) * speed;

                if (Math.Abs(position.X) > distance)
                {
                    direction *= -1;
                }

                if (blueSnailBodyData.Body != null)
                {
                    var body = blueSnailBodyData.Body;
                    body.SetXForm(position.FromVector2(), body.GetAngle());
                }

                yield return null;
            }
        }
    }
}