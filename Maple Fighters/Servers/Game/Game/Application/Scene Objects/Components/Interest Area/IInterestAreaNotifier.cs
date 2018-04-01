using CommonCommunicationInterfaces;

namespace Game.Application.SceneObjects.Components
{
    public interface IInterestAreaNotifier
    {
        void NotifySubscribers<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters;

        void NotifySubscriberOnly<TParameters>(int peerId, byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters;
    }
}