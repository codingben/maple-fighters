using Game.Common;
using Game.InterestManagement;

namespace Game.Application.Components
{
    internal interface ICharacterCreator
    {
        ISceneObject Create(CharacterParameters character);
        void CreateCharacterBody(IGameSceneWrapper sceneWrapper, ISceneObject sceneObject);
    }
}