using Game.Common;

namespace Scripts.Gameplay.Actors
{
    public interface ICharacterCreator
    {
        void Create(CharacterSpawnDetailsParameters characterSpawnDetails);
    }
}