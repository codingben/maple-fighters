using System.Collections.Generic;

namespace Scripts.World
{
    public interface IDummySceneObjectsProvider
    {
        IEnumerable<DummySceneObject> GetSceneObjects();
    }
}