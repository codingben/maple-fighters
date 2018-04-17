using CommonTools.Log;
using Scripts.Containers;
using Scripts.Utils;
using Game.Common;
using Scripts.Services;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreator : DontDestroyOnLoad<CharacterCreator>
    {
        private void Start()
        {
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.SceneEntered.AddListener(OnSceneEntered);
            gameScenePeerLogic.CharacterAdded.AddListener(OnCharacterAdded);
            gameScenePeerLogic.CharactersAdded.AddListener(OnCharactersAdded);
        }

        private void UnsubscribeFromEvents()
        {
            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.SceneEntered.RemoveListener(OnSceneEntered);
            gameScenePeerLogic.CharacterAdded.RemoveListener(OnCharacterAdded);
            gameScenePeerLogic.CharactersAdded.RemoveListener(OnCharactersAdded);
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