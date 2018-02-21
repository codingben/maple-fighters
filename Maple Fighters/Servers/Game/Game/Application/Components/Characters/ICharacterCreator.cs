using Characters.Client.Common;
using ComponentModel.Common;
using Game.InterestManagement;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal interface ICharacterCreator : IExposableComponent
    {
        ISceneObject Create(CharacterFromDatabaseParameters character);

        void CreateCharacterBody(IGameSceneWrapper sceneWrapper, ISceneObject sceneObject);
    }
}