namespace Game.Application.Objects.Components
{
    public interface ICharacterData
    {
        void SetName(string name);

        void SetCharacterType(byte characterType);

        string GetName();

        byte GetCharacterType();
    }
}