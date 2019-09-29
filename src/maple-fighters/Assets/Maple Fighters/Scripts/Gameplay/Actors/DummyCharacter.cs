using System;
using Game.Common;
using Scripts.Editor;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [Serializable]
    public class DummyCharacter
    {
        [ViewOnly]
        public int Id;
        public string Name;
        public CharacterClasses CharacterClass;
        public Vector2 Position;
        public Directions Direction;

        public static EnterSceneResponseParameters CreateDummyCharacter(
            int id,
            string name,
            CharacterClasses characterClass,
            Vector2 position,
            Directions direction)
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