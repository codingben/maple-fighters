using System;
using System.Collections.Generic;

namespace InterestManagement
{
    public interface IInterestArea<TSceneObject> : IDisposable
        where TSceneObject : ISceneObject
    {
        IEnumerable<IRegion<TSceneObject>> GetRegions();

        IEnumerable<TSceneObject> GetNearbySceneObjects();

        INearbySceneObjectsEvents<TSceneObject> GetNearbySceneObjectsEvents();
    }
}