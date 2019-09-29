using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class DummyCharacterDetailsProvider : MonoBehaviour
    {
        [SerializeField]
        private DummyCharacter dummyCharacter;

        public EnterSceneResponseParameters GetDummyCharacterParameters()
        {
            return DummyCharacter.CreateDummyCharacter(
                dummyCharacter.Id,
                dummyCharacter.Name,
                dummyCharacter.CharacterClass,
                dummyCharacter.Position,
                dummyCharacter.Direction);
        }
    }
}