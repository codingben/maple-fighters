using System;
using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public interface ICharacterCreator
    {
        // The character game object, and the sprite game object
        event Action<GameObject, GameObject> OnCharacterCreated;

        void CreateCharacter(CharacterSpawnDetailsParameters characterSpawnDetails);
    }
}