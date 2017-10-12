using System;
using Scripts.Gameplay;
using UnityEngine;

namespace Scripts.Containers
{
    public interface IGameObjectsContainer
    {
        event Action GameObjectsAdded;

        IGameObject GetLocalGameObject();

        GameObject GetRemoteGameObject(int id);
    }
}