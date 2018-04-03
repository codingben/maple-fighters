using System.Collections.Generic;
using Box2DX.Collision;
using Box2DX.Dynamics;
using CommonTools.Coroutines;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.SceneObjects.Components.Interfaces;
using MathematicsHelper;
using Game.Common;
using InterestManagement;
using InterestManagement.Components.Interfaces;
using Physics.Box2D.Components.Interfaces;
using Physics.Box2D.Core;

namespace Game.Application.SceneObjects.Components
{
    internal class CharacterBody : Component<ISceneObject>, ICharacterBody
    {
        public PlayerState PlayerState { private get; set; }

        private Body body;
        private Vector2 lastPosition;

        private IScene scene;
        private IPositionTransform positionTransform;

        protected override void OnAwake()
        {
            base.OnAwake();

            positionTransform = Entity.Components.GetComponent<IPositionTransform>().AssertNotNull();

            var presenceSceneProvider = Entity.Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            scene = presenceSceneProvider.Scene;

            var executor = scene.Components.GetComponent<ISceneOrderExecutor>().AssertNotNull();
            executor.GetPreUpdateExecutor().StartCoroutine(UpdatePosition());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            var entityManager = scene?.Components.GetComponent<IEntityManager>().AssertNotNull();
            entityManager?.RemoveBody(body, Entity.Id);
        }

        private IEnumerator<IYieldInstruction> UpdatePosition()
        {
            var entityManager = scene.Components.GetComponent<IEntityManager>().AssertNotNull();

            while (true)
            {
                if (body == null)
                {
                    body = entityManager.GetBody(Entity.Id);
                }
                else
                {
                    SetPosition();
                }
                yield return null;
            }
        }

        private void SetPosition()
        {
            if (Vector2.Distance(positionTransform.Position, lastPosition) < 0.1f)
            {
                return;
            }

            switch (PlayerState)
            {
                case PlayerState.Idle:
                case PlayerState.Moving:
                {
                    if (body.GetMass() == 0)
                    {
                        body.SetMassFromShapes();
                    }
                    else
                    {
                        /* 
                         * NOTE: Deprecated MoveBody() due to forces and velocity issues between two fixtures.
                           -> const float SPEED = 10.5f; // TODO: Get this data from another source
                           -> body.MoveBody(transform.Position, SPEED);
                        */

                        body.SetXForm(positionTransform.Position.FromVector2(), body.GetAngle());
                    }
                    break;
                }
                case PlayerState.Falling:
                case PlayerState.Rope:
                case PlayerState.Ladder:
                {
                    if (body.GetMass() > 0)
                    {
                        body.SetMass(new MassData());
                    }
                    else
                    {
                        body.SetXForm(positionTransform.Position.FromVector2(), body.GetAngle());
                    }
                    break;
                }
            }

            lastPosition = positionTransform.Position;
        }
    }
}