using MathematicsHelper;

namespace Game.Entity.Components
{
    internal interface ITransform // (Will be inside an IComponentsContainer as a component; Inside in a Entity)
    {
        void SetPosition(Vector2 position);

        Vector2 GetPosition();
    }
}