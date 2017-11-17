using System.Collections.Generic;
using Box2DX.Collision;
using Box2DX.Dynamics;
using CommonTools.Coroutines;
using CommonTools.Log;
using ComponentModel.Common;
using Game.InterestManagement;
using Physics.Box2D;
using Shared.Game.Common;

namespace Game.Application.SceneObjects.Components
{
    internal class CharacterBody : Component<ISceneObject>, ICharacterBody
    {
        public PlayerState PlayerState { private get; set; }
        private Body body;

        protected override void OnAwake()
        {
            base.OnAwake();

            var entityManager = Entity.Scene.Container.GetComponent<IEntityManager>().AssertNotNull();
            body = entityManager.GetBody(Entity.Id).AssertNotNull();

            var executor = Entity.Scene.Container.GetComponent<ISceneOrderExecutor>().AssertNotNull();
            executor.GetPreUpdateExecutor().StartCoroutine(UpdatePosition());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Entity.AssertNotNull(MessageBuilder.Trace("Entity is null."));
            Entity?.Scene.AssertNotNull(MessageBuilder.Trace("Scene is null."));

            if (Entity?.Scene == null)
            {
                return;
            }

            var entityManager = Entity.Scene.Container.GetComponent<IEntityManager>().AssertNotNull();
            entityManager.RemoveBody(Entity.Id);
        }

        private IEnumerator<IYieldInstruction> UpdatePosition()
        {
            var transform = Entity.Container.GetComponent<ITransform>().AssertNotNull();

            while (true)
            {
                if (body == null)
                {
                    yield return null;
                }

                switch (PlayerState)
                {
                    case PlayerState.Idle:
                    case PlayerState.Moving:
                    {
                        if (body.GetMass() == 0)
                        {
                            body.SetMassFromShapes();
                            yield return null;
                        }

                        const float SPEED = 10.5f; // TODO: Get this data from another source
                        body.MoveBody(transform.Position, SPEED);
                        break;
                    }
                    case PlayerState.Falling:
                    case PlayerState.Rope:
                    case PlayerState.Ladder:
                    {
                        if (body.GetMass() > 0)
                        {
                            body.SetMass(new MassData());
                            yield return null;
                        }

                        body.SetXForm(transform.Position.FromVector2(), body.GetAngle());
                        break;
                    }
                }
                yield return null;
            }
        }
    }
}