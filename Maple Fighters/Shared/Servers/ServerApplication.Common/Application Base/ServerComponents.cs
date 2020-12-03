using ComponentModel.Common;

namespace ServerApplication.Common.ApplicationBase
{
    public static class ServerComponents
    {
        private static readonly IContainer Components = new Container();

        public static T AddComponent<T>(T component)
            where T : IComponent
        {
            return Components.AddComponent(component);
        }

        public static void RemoveComponent<T>()
            where T : IComponent
        {
            Components.RemoveComponent<T>();
        }

        public static T GetComponent<T>()
            where T : class
        {
            return Components.GetComponent<T>();
        }

        public static void RemoveAllComponents()
        {
            Components.Dispose();
        }
    }
}