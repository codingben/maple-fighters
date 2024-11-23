namespace Scripts.UI.PlayerData
{
    public interface IPlayerDataView
    {
        void SetLevel(int level);

        void SetName(string name);

        void SetHealthPoints(int value);

        void SetMaxHealthPoints(int value);
    }
}