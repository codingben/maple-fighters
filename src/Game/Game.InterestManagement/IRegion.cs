using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public interface IRegion
    {
        bool Subscribe(ISceneObject sceneObject);

        bool Unsubscribe(ISceneObject sceneObject);

        Rectangle GetRectangle();

        IEnumerable<ISceneObject> GetAllSceneObjects();
    }
}