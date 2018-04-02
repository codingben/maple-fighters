using Game.Common;
using InterestManagement.Components.Interfaces;

namespace Game.Application.Components.Interfaces
{
    internal interface ICharacterCreator
    {
        ISceneObject Create(CharacterParameters character);
        void CreateCharacterBody(IGameSceneWrapper sceneWrapper, ISceneObject sceneObject);
    }
}