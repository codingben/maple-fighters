namespace Common.ComponentModel
{
    public interface IExposableComponentsContainer
    {
        TComponent Add<TComponent>(TComponent component)
            where TComponent : class;

        TComponent Get<TComponent>()
            where TComponent : class;
    }
}