using System;
using UnityEngine;

namespace Scripts.Gameplay.PlayerCharacter
{
    public interface ISpawnedCharacter
    {
        event Action CharacterSpawned;

        GameObject GetCharacterGameObject();

        GameObject GetCharacterSpriteGameObject();
    }
}