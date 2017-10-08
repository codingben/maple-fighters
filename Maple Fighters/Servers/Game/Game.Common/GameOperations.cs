namespace Shared.Game.Common
{
    public enum GameOperations : byte
    {
        Authenticate,
        FetchCharacters,
        EnterWorld,
        PositionChanged,
        PlayerStateChanged,
        ChangeScene,
        CreateCharacter,
        RemoveCharacter
    }
}