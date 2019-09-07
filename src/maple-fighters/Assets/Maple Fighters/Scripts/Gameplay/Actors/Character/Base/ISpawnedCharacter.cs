using System;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public interface ISpawnedCharacter
    {
        event Action CharacterSpawned;

        GameObject GetCharacterGameObject();

        GameObject GetCharacterSpriteGameObject();
    }
}