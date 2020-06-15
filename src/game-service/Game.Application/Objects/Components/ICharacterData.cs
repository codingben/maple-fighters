namespace Game.Application.Objects.Components
{
    public interface ICharacterData
    {
        void Set(string name, byte characterType);

        string GetName();

        byte GetCharacterType();
    }
}