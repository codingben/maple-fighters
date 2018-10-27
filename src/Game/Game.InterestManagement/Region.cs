using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public class Region : IRegion
    {
        private readonly Rectangle rectangle;
        private readonly HashSet<ISceneObject> sceneObjects = new HashSet<ISceneObject>();

        public Region(Vector2 position, Vector2 size)
        {
            rectangle = new Rectangle(position, size);
        }

        public bool Subscribe(ISceneObject sceneObject)
        {
            return sceneObjects.Add(sceneObject);
        }

        public bool Unsubscribe(ISceneObject sceneObject)
        {
            return sceneObjects.Remove(sceneObject);
        }

        public Rectangle GetRectangle()
        {
            return rectangle;
        }

        public IEnumerable<ISceneObject> GetAllSceneObjects()
        {
            return sceneObjects;
        }
    }
}