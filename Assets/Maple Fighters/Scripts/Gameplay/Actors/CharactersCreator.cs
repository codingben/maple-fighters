using System.Threading.Tasks;
using CommonTools.Coroutines;
using Scripts.Containers;
using Scripts.Utils;
using Shared.Game.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public class CharactersCreator : DontDestroyOnLoad<CharactersCreator>
    {
        [SerializeField] private DummyCharacter dummyCharacter;
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        protected override void OnAwake()
        {
            base.OnAwake();

            SubscribeToEvents();
        }

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor.Dispose();

            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            ServiceContainer.GameService.CharacterAdded.AddListener(OnCharacterAdded);
            ServiceContainer.GameService.CharactersAdded.AddListener(OnCharactersAdded);
        }

        private void UnsubscribeFromEvents()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            ServiceContainer.GameService.CharacterAdded.RemoveListener(OnCharacterAdded);
            ServiceContainer.GameService.CharactersAdded.RemoveListener(OnCharactersAdded);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            coroutinesExecutor.StartTask(EnterScene);
        }

        private async Task EnterScene(IYield yield)
        {
            var parameters = await ServiceContainer.GameService.EnterScene(yield);
            if (!parameters.HasValue)
            {
                parameters = DummyCharacter.CreateDummyCharacter(dummyCharacter.Id, dummyCharacter.Name, dummyCharacter.CharacterClass, 
                    dummyCharacter.spawnPosition, dummyCharacter.spawnDirection);
            }

            // Will create scene object.
            ServiceContainer.GameService.EnteredScene.Invoke(parameters.Value);

            // Will create a character for this scene object.
            var characterSpawnDetails = parameters.Value.Character;
            CreateCharacter(characterSpawnDetails);
        }

        private void OnCharacterAdded(CharacterAddedEventParameters parameters)
        {
            var characterSpawnDetails = parameters.CharacterSpawnDetails;
            CreateCharacter(characterSpawnDetails);
        }

        private void OnCharactersAdded(CharactersAddedEventParameters parameters)
        {
            foreach (var characterSpawnDetails in parameters.CharactersSpawnDetails)
            {
                CreateCharacter(characterSpawnDetails);
            }
        }

        private void CreateCharacter(CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            var id = characterSpawnDetails.SceneObjectId;
            var sceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(id);
            sceneObject?.GetGameObject().GetComponent<ICharacterCreator>().Create(characterSpawnDetails);
        }
    }
}