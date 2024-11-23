namespace Scripts.UI.PlayerData
{
    public interface IPlayerExperiencePointsView
    {
        void AddExperiencePoints(float value);

        void SetExperiencePoints(float value);

        void SetMaxExperiencePoints(float value);
    }
}