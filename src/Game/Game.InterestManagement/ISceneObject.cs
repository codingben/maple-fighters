namespace Game.InterestManagement
{
    public interface ISceneObject
    {
        int Id { get; }

        ITransform Transform { get; }
    }
}