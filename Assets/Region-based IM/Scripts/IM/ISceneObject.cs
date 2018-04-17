using UnityEngine;

namespace InterestManagement.Scripts
{
    public interface ISceneObject
    {
        int Id { get; }

        GameObject GetGameObject();
    }
}