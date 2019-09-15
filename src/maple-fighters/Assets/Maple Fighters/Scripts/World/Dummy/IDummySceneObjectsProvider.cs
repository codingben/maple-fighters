using System.Collections.Generic;

namespace Scripts.World.Dummy
{
    public interface IDummySceneObjectsProvider
    {
        IEnumerable<DummySceneObject> GetSceneObjects();
    }
}