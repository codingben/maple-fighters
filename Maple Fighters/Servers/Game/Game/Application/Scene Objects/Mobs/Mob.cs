using CommonTools.Log;
using Game.InterestManagement;
using MathematicsHelper;
using Physics.Box2D;

namespace Game.Application.SceneObjects
{
    /// <summary>
    /// Will create a body (and fixture) in the physics world and will inform about a collision detection.
    /// </summary>
    public class Mob : SceneObject
    {
        public Mob(string name, Vector2 position, Vector2 size, float direction) 
            : base(name, position, direction)
        {
            var physicsWorldProvider = Scene.Container.GetComponent<IPhysicsWorldProvider>().AssertNotNull();
            var world = physicsWorldProvider.GetWorld();
            var physicsCollisionProvider = Container.AddComponent(new PhysicsCollisionNotifier());
            var body = world.CreateCharacter(position, size, LayerMask.Mob, physicsCollisionProvider);
            body.SetUserData(this);
        }
    }
}