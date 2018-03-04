using Game.Common;
using Scripts.UI.Core;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class DummyCharacterDetails : MonoSingleton<DummyCharacterDetails>
    {
        [SerializeField] private DummyCharacter dummyCharacter;

        public EnterSceneResponseParameters GetDummyCharacterParameters() =>
            DummyCharacter.CreateDummyCharacter(dummyCharacter.Id, dummyCharacter.Name, dummyCharacter.CharacterClass, dummyCharacter.spawnPosition, dummyCharacter.spawnDirection);

        private void Start()
        {
            Destroy(gameObject);
        }
    }
}