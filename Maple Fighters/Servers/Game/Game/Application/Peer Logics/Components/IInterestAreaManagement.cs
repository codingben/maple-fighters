using CommonCommunicationInterfaces;
using ComponentModel.Common;

namespace Game.Application.PeerLogic.Components
{
    internal interface IInterestAreaManagement : IExposableComponent
    {
        void SendEventForSubscribers<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters;
    }
}