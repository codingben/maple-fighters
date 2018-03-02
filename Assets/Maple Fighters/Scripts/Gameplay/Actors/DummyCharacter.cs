using Character.Client.Common;
using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [System.Serializable]
    public class DummyCharacter
    {
        public int Id;
        public string Name;
        public CharacterClasses CharacterClass;
        public Vector2 spawnPosition;
        public Directions spawnDirection;

        public static EnterSceneResponseParameters CreateDummyCharacter(int id, string name, CharacterClasses characterClass, 
            Vector2 spawnPosition, Directions spawnDirection)
        {
            const string OBJECT_FROM_GAME_RESOURCES = "Local Player";

            var sceneObject = new SceneObjectParameters(id, OBJECT_FROM_GAME_RESOURCES, spawnPosition.x, spawnPosition.y);
            var characterFromDatabase = new CharacterFromDatabaseParameters(name, characterClass, CharacterIndex.Zero);
            var character = new CharacterSpawnDetailsParameters(sceneObject.Id, characterFromDatabase, spawnDirection);
            return new EnterSceneResponseParameters(sceneObject, character);
        }
    }
}