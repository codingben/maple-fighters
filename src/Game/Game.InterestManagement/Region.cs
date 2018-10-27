using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public class Region : IRegion
    {
        public Rectangle Rectangle { get; }

        private readonly HashSet<ISceneObject> sceneObjects = new HashSet<ISceneObject>();

        public Region(Vector2 position, Vector2 size)
        {
            Rectangle = new Rectangle(position, size);
        }

        public void AddSceneObject(ISceneObject sceneObject)
        {
            sceneObjects.Add(sceneObject);
        }

        public void RemoveSceneObject(ISceneObject sceneObject)
        {
            sceneObjects.Remove(sceneObject);
        }
    }
}