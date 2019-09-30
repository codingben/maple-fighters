using Game.Common;
using Scripts.Editor;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class DummyCharacterDetailsProvider : MonoBehaviour
    {
        [ViewOnly, SerializeField]
        private int id;

        [SerializeField]
        private string name;

        [SerializeField]
        private CharacterClasses characterClass;

        [SerializeField]
        private Vector2 position;

        [SerializeField]
        private Directions direction;

        public EnterSceneResponseParameters GetDummyCharacter()
        {
            var sceneObject = new SceneObjectParameters(
                id,
                "Local Player",
                position.x,
                position.y,
                direction);

            var character = new CharacterSpawnDetailsParameters(
                id,
                new CharacterParameters(
                    name,
                    characterClass,
                    CharacterIndex.Zero),
                direction);

            return new EnterSceneResponseParameters(sceneObject, character);
        }
    }
}