using Game.Entities;
using Game.Entity.Components;
using MathematicsHelper;

namespace Game.InterestManagement
{
    internal class InterestArea : EntityComponent, IInterestArea
    {
        public InterestArea(IEntity entity) 
            : base(entity)
        {
            // Left blank intentionally
        }

        public void SetPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        public void SetSize(Vector2 size)
        {
            throw new System.NotImplementedException();
        }

        public Vector2 GetPosition()
        {
            throw new System.NotImplementedException();
        }

        public Vector2 GetSize()
        {
            throw new System.NotImplementedException();
        }

        public IRegion GetRegion()
        {
            throw new System.NotImplementedException();
        }
    }
}