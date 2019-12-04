using System;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public interface ISpawnedCharacter
    {
        event Action CharacterSpawned;

        GameObject GetCharacterGameObject();

        GameObject GetCharacterSpriteGameObject();
    }
}