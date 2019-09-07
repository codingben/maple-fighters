using Game.Common;

namespace Scripts.Gameplay.Actors
{
    public interface ISpawnedCharacterDetails
    {
        CharacterSpawnDetailsParameters GetCharacterDetails();
    }
}