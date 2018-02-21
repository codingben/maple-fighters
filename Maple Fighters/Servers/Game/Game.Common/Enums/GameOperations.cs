namespace Shared.Game.Common
{
    public enum GameOperations : byte
    {
        Authenticate,
        EnterScene,
        ChangeScene,
        PositionChanged,
        PlayerStateChanged,
        ValidateCharacter,
    }
}