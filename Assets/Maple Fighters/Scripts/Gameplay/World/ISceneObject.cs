using UnityEngine;

namespace Scripts.Gameplay
{
    public interface ISceneObject
    {
        int Id { get; set; }

        GameObject GameObject { get; }
    }
}