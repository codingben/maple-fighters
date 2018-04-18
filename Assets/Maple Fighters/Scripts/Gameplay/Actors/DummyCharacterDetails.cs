using Game.Common;
using Scripts.UI.Core;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    // TODO: Remove
    public class DummyCharacterDetails : MonoSingleton<DummyCharacterDetails>
    {
        [SerializeField] private DummyCharacter dummyCharacter;

        public EnterSceneResponseParameters GetDummyCharacterParameters()
        {
            Destroy(gameObject);

            return DummyCharacter.CreateDummyCharacter(dummyCharacter.Id, dummyCharacter.Name, dummyCharacter.CharacterClass,
                dummyCharacter.spawnPosition, dummyCharacter.spawnDirection);
        }
    }
}