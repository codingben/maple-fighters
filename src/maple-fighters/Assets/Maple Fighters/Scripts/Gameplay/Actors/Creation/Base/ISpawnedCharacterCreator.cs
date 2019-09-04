using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public interface ISpawnedCharacterCreator
    {
        GameObject Create(Transform parent, CharacterClasses characterClass);
    }
}