using Game.Common;
using Scripts.Editor;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [System.Serializable]
    public class DummyCharacter
    {
        [ViewOnly] public int Id = 0;
        public string Name;
        public CharacterClasses CharacterClass;
        public Vector2 SpawnPosition;
        public Directions SpawnDirection;

        public static EnterSceneResponseParameters CreateDummyCharacter(int id, string name, CharacterClasses characterClass, 
            Vector2 spawnPosition, Directions spawnDirection)
        {
            const string OBJECT_FROM_GAME_RESOURCES = "Local Player";

            var sceneObject = new SceneObjectParameters(id, OBJECT_FROM_GAME_RESOURCES, spawnPosition.x, spawnPosition.y, spawnDirection);
            var characterFromDatabase = new CharacterParameters(name, characterClass, CharacterIndex.Zero);
            var character = new CharacterSpawnDetailsParameters(sceneObject.Id, characterFromDatabase, spawnDirection);
            return new EnterSceneResponseParameters(sceneObject, character);
        }
    }
}