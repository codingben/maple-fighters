using Shared.Game.Common;

namespace Scripts.Gameplay.Actors
{
    public interface ICharacterCreator
    {
        void Create(CharacterSpawnDetails characterSpawnDetails);
    }
}