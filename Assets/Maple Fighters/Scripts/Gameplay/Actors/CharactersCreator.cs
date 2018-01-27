using System.Threading.Tasks;
using CommonTools.Coroutines;
using Scripts.Containers;
using Scripts.Utils;
using Shared.Game.Common;
using UnityEngine.SceneManagement;

namespace Scripts.Gameplay.Actors
{
    public class CharactersCreator : DontDestroyOnLoad<CharactersCreator>
    {
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
            ServiceContainer.GameService.EnteredScene.Invoke(parameters);

            CreateLocalCharacter(parameters.Character);
        }

        private void CreateLocalCharacter(CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            var sceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(characterSpawnDetails.SceneObjectId);
            var characterInformationProvider = sceneObject.GetGameObject().GetComponent<CharacterInformationProvider>();
            characterInformationProvider.SetCharacterInformation(characterSpawnDetails.Character);

            sceneObject.GetGameObject().GetComponent<ICharacterCreator>().Create(characterSpawnDetails);
        }

        private void OnCharacterAdded(CharacterAddedEventParameters parameters)
        {
            var id = parameters.CharacterSpawnDetails.SceneObjectId;
            var sceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(id);

            var character = parameters.CharacterSpawnDetails;
            sceneObject.GetGameObject().GetComponent<ICharacterCreator>().Create(character);
        }

        private void OnCharactersAdded(CharactersAddedEventParameters parameters)
        {
            foreach (var characterSpawnDetails in parameters.CharactersSpawnDetails)
            {
                var id = characterSpawnDetails.SceneObjectId;
                var sceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(id);
                sceneObject.GetGameObject().GetComponent<ICharacterCreator>().Create(characterSpawnDetails);
            }
        }
    }
}