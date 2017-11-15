using Box2DX.Dynamics;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using MathematicsHelper;
using Physics.Box2D;
using Shared.Game.Common;
using SceneObject = Game.InterestManagement.SceneObject;

namespace Game.Application.SceneObjects
{
    /// <summary>
    /// Will create a body (and fixture) in the physics world and will inform about a collision detection.
    /// </summary>
    public class Mob : SceneObject
    {
        protected Body Body;
        private readonly Vector2 bodySize;
        private IInterestAreaNotifier interestAreaNotifier;

        protected Mob(string name, Vector2 position, Vector2 size, float direction) 
            : base(name, position, direction)
        {
            bodySize = size;
        }

        protected void CreateCharacter()
        {
            var transform = Container.GetComponent<ITransform>().AssertNotNull();
            transform.PositionDirectionChanged += OnPositionChanged;

            var physicsWorldProvider = Scene.Container.GetComponent<IPhysicsWorldProvider>().AssertNotNull();
            var world = physicsWorldProvider.GetWorld();

            var physicsCollisionProvider = Container.AddComponent(new PhysicsCollisionNotifier());
            Body = world.CreateCharacter(transform.Position, bodySize, LayerMask.Mob, physicsCollisionProvider);
            Body.SetUserData(this);

            interestAreaNotifier = Container.AddComponent(new InterestAreaNotifier());
        }

        private void OnPositionChanged(Vector2 position, Directions direction)
        {
            const float SPEED = 2.5f; // TODO: Get it from an another source
            Body.MoveBody(position, SPEED, false);

            var parameters = new SceneObjectPositionChangedEventParameters(Id, position.X, position.Y, direction);
            var messageSendOptions = MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position);
            interestAreaNotifier.NotifySubscribers((byte)GameEvents.PositionChanged, parameters, messageSendOptions);
        }
    }
}