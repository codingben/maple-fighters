using CommonCommunicationInterfaces;
using ComponentModel.Common;
using Game.InterestManagement;

namespace Game.Application.SceneObjects.Components
{
    public interface IInterestAreaNotifier : IExposableComponent
    {
        void NotifySubscribers<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters;

        void NotifySubscriberOnly<TParameters>(ISceneObject subscriber, byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters;
    }
}