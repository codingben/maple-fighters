using CommonCommunicationInterfaces;

namespace Scripts.Network.APIs
{
    public static class ExtensionMethods
    {
        public static EventHandler<TParameters> ToHandler<TParameters>(this UnityEvent<TParameters> action)
            where TParameters : struct, IParameters
        {
            return new EventHandler<TParameters>((messageData) => action?.Invoke(messageData.Parameters));
        }
    }
}