using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public class Region : IRegion
    {
        public IRectangle Rectangle { get; }

        private readonly HashSet<ISceneObject> sceneObjects = new HashSet<ISceneObject>();

        public Region(Vector2 position, Vector2 size)
        {
            Rectangle = new Rectangle(position, size);
        }

        public void Subscribe(ISceneObject sceneObject)
        {
            sceneObjects.Add(sceneObject);
        }

        public void Unsubscribe(ISceneObject sceneObject)
        {
            sceneObjects.Remove(sceneObject);
        }

        public IEnumerable<ISceneObject> GetAllSubscribers()
        {
            return sceneObjects;
        }
    }
}