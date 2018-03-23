using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.Utils;
using Game.Common;
using Scripts.Services;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreator : DontDestroyOnLoad<CharacterCreator>
    {
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Start()
        {
            SubscribeToEvents();

            if (!ServiceContainer.GameService.ServiceConnectionHandler.IsConnected())
            {
                CreateDummyCharacter();
            }
        }

        private void CreateDummyCharacter()
        {
            var parameters = DummyCharacterDetails.Instance.AssertNotNull("Could not find dummy character details. Please add DummyCharacterDetails into a scene.")
                .GetDummyCharacterParameters();

            var gameService = ServiceContainer.GameService.GetPeerLogic<IGameServiceAPI>().AssertNotNull();
            gameService.SceneEntered?.Invoke(parameters);
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
            var gameService = ServiceContainer.GameService.GetPeerLogic<IGameServiceAPI>().AssertNotNull();
            gameService.SceneEntered.AddListener(OnSceneEntered);
            gameService.CharacterAdded.AddListener(OnCharacterAdded);
            gameService.CharactersAdded.AddListener(OnCharactersAdded);
        }

        private void UnsubscribeFromEvents()
        {
            var gameService = ServiceContainer.GameService.GetPeerLogic<IGameServiceAPI>().AssertNotNull();
            gameService.SceneEntered.RemoveListener(OnSceneEntered);
            gameService.CharacterAdded.RemoveListener(OnCharacterAdded);
            gameService.CharactersAdded.RemoveListener(OnCharactersAdded);
        }

        private void OnSceneEntered(EnterSceneResponseParameters parameters)
        {
            var characterSpawnDetails = parameters.Character;
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
            var sceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(id).AssertNotNull();
            sceneObject?.GetGameObject().GetComponent<ICharacterCreator>().Create(characterSpawnDetails);
        }
    }
}