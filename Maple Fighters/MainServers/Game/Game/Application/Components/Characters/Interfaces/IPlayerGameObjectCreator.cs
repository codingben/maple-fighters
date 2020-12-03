using Game.Application.GameObjects;
using Game.Common;

namespace Game.Application.Components.Interfaces
{
    internal interface IPlayerGameObjectCreator
    {
        PlayerGameObject Create(CharacterParameters character);
    }
}