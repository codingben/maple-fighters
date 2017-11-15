using CommonCommunicationInterfaces;
using ComponentModel.Common;

namespace Game.Application.SceneObjects.Components
{
    internal interface IInterestAreaNotifier : IExposableComponent
    {
        void NotifySubscribers<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters;
    }
}