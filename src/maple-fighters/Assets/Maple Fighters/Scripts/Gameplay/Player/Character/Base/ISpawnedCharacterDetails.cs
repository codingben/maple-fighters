using Game.Common;

namespace Scripts.Gameplay.PlayerCharacter
{
    public interface ISpawnedCharacterDetails
    {
        CharacterSpawnDetailsParameters GetCharacterDetails();
    }
}