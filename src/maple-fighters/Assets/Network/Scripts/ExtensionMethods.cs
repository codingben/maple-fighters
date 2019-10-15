using CommonCommunicationInterfaces;

namespace Network.Scripts
{
    public static class ExtensionMethods
    {
        public static EventHandler<TParameters> ToEventHandler<TParameters>(this UnityEvent<TParameters> action)
            where TParameters : struct, IParameters
        {
            return new EventHandler<TParameters>((x) => action?.Invoke(x.Parameters));
        }
    }
}