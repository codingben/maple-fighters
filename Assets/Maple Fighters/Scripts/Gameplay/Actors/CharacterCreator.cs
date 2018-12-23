using CommonTools.Log;
using Game.Common;
using Scripts.Containers;
using Scripts.Services;
using Scripts.Utils;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreator : MonoSingleton<CharacterCreator>
    {
        private void Start()
        {
            SubscribeToEvents();
        }
        
        protected override void OnDestroying()
        {
            base.OnDestroying();

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
            var sceneObject = SceneObjectsContainer.GetInstance().GetRemoteSceneObject(id).AssertNotNull();
            sceneObject?.GameObject.GetComponent<ICharacterCreator>().Create(characterSpawnDetails);
        }
    }
}