using MathematicsHelper;

namespace Game.InterestManagement
{
    internal interface IInterestArea // (Will be inside an IComponentsContainer as a component; Inside in a Entity)
    {
        void SetPosition(Vector2 position);
        void SetSize(Vector2 size);

        Vector2 GetPosition();
        Vector2 GetSize();

        IRegion GetRegion();
    }
}