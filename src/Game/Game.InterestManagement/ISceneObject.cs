namespace Game.InterestManagement
{
    public interface ISceneObject
    {
        int Id { get; }

        IScene Scene { get; }

        ITransform Transform { get; }
    }
}