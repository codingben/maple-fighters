using Character.Client.Common;
using ComponentModel.Common;
using Game.InterestManagement;

namespace Game.Application.Components
{
    internal interface ICharacterCreator : IExposableComponent
    {
        ISceneObject Create(CharacterFromDatabaseParameters character);
        void CreateCharacterBody(IGameSceneWrapper sceneWrapper, ISceneObject sceneObject);
    }
}