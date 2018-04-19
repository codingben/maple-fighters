using System.Collections.Generic;
using Box2DX.Collision;
using Box2DX.Dynamics;
using CommonTools.Coroutines;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.GameObjects.Components.Interfaces;
using MathematicsHelper;
using Game.Common;
using InterestManagement;
using InterestManagement.Components.Interfaces;
using Physics.Box2D.Components.Interfaces;
using Physics.Box2D.Core;

namespace Game.Application.GameObjects.Components
{
    internal class PlayerPositionController : Component<ISceneObject>, IPlayerPositionController
    {
        public PlayerState PlayerState { private get; set; }

        private Body body;
        private Vector2 lastPosition;

        private IPositionTransform positionTransform;
        private IPresenceSceneProvider presenceSceneProvider;
        private ICoroutine updatePosition;

        protected override void OnAwake()
        {
            base.OnAwake();

            positionTransform = Entity.Components.GetComponent<IPositionTransform>().AssertNotNull();
            presenceSceneProvider = Entity.Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();

            var presenceSceneChangesNotifier = Entity.Components.GetComponent<IPresenceSceneChangesNotifier>().AssertNotNull();
            presenceSceneChangesNotifier.SceneChanged += OnSceneChanged;

            var executor = presenceSceneProvider.GetScene().Components.GetComponent<ISceneOrderExecutor>().AssertNotNull();
            updatePosition = executor.GetPreUpdateExecutor().StartCoroutine(UpdatePosition());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            var presenceSceneChangesNotifier = Entity.Components.GetComponent<IPresenceSceneChangesNotifier>().AssertNotNull();
            presenceSceneChangesNotifier.SceneChanged -= OnSceneChanged;
        }

        private void OnSceneChanged(IScene scene)
        {
            if (scene == null)
            {
                updatePosition.Dispose();
                return;
            }

            var executor = presenceSceneProvider.GetScene().Components.GetComponent<ISceneOrderExecutor>().AssertNotNull();
            updatePosition = executor.GetPreUpdateExecutor().StartCoroutine(UpdatePosition());
        }

        private IEnumerator<IYieldInstruction> UpdatePosition()
        {
            yield return null; // Hack

            var entityManager = presenceSceneProvider.GetScene().Components.GetComponent<IEntityManager>().AssertNotNull();
            body = entityManager.GetBody(Entity.Id).AssertNotNull("Could not find a body.");

            while (true)
            {
                if (body != null)
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
                    MovingState();
                    break;
                }
                case PlayerState.Jumping:
                case PlayerState.Falling:
                case PlayerState.Rope:
                case PlayerState.Ladder:
                {
                    LadderState();
                    break;
                }
            }

            lastPosition = positionTransform.Position;

            void MovingState()
            {
                if (body.GetMass() == 0)
                {
                    body.SetMassFromShapes();
                }
                else
                {
                    body.SetXForm(positionTransform.Position.FromVector2(), body.GetAngle());
                }
            }

            void LadderState()
            {
                if (body.GetMass() > 0)
                {
                    body.SetMass(new MassData());
                }
                else
                {
                    body.SetXForm(positionTransform.Position.FromVector2(), body.GetAngle());
                }
            }
        }
    }
}