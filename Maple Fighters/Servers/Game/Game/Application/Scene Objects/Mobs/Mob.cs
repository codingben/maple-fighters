using Game.Application.SceneObjects.Components;
using MathematicsHelper;
using SceneObject = Game.InterestManagement.SceneObject;

namespace Game.Application.SceneObjects
{
    /// <summary>
    /// Will create a body (and fixture) in the physics world and will inform about a collision detection.
    /// </summary>
    public class Mob : SceneObject
    {
        protected readonly IInterestAreaNotifier InterestAreaNotifier;

        protected Mob(string name, Vector2 position) 
            : base(name, position, 1)
        {
            InterestAreaNotifier = Container.AddComponent(new InterestAreaNotifier());
        }
    }
}