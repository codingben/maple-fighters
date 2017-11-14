using ComponentModel.Common;
using Game.InterestManagement;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal interface ICharacterCreator : IExposableComponent
    {
        ISceneObject Create(Character character);

        void CreateCharacterBody(IGameSceneWrapper sceneWrapper, ISceneObject sceneObject);
    }
}