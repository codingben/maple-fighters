using System.Collections.Generic;

namespace Scripts.World.Dummy
{
    public interface IDummyEntitiesProvider
    {
        IEnumerable<DummyEntity> GetEntities();
    }
}