using System.Collections.Generic;

namespace Assets.Scripts
{
    public interface IDummySceneObjectsProvider
    {
        IEnumerable<DummySceneObject> GetSceneObjects();
    }
}