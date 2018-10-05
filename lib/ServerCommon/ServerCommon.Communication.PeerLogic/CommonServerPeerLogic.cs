using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;

namespace ServerCommon.Communication.PeerLogic
{
    public sealed class CommonServerPeerLogic<TOperationCode, TEventCode> :
        OutboundPeerLogicBase<TOperationCode, TEventCode>,
        IServerPeerLogic<TOperationCode, TEventCode>
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        public void SendOperation<TParameters>(
            TOperationCode code,
            TParameters parameters)
            where TParameters : struct, IParameters
        {
            OperationRequestSender.Send(
                code,
                parameters,
                MessageSendOptions.DefaultReliable());
        }

        public async Task<TResponseParameters> SendOperationAsync<TRequestParameters, TResponseParameters>(
            IYield yield,
            TOperationCode code,
            TRequestParameters parameters)
            where TRequestParameters : struct, IParameters
            where TResponseParameters : struct, IParameters
        {
            var id = OperationRequestSender.Send(
                code,
                parameters,
                MessageSendOptions.DefaultReliable());

            return await SubscriptionProvider
                       .ProvideSubscription<TResponseParameters>(yield, id);
        }

        public void SetEventHandler<TEventParameters>(
            TEventCode code,
            Action<TEventParameters> action)
            where TEventParameters : struct, IParameters
        {
            EventHandlerRegister.SetHandler(
                code,
                new EventHandler<TEventParameters>(
                    x => action.Invoke(x.Parameters)));
        }

        public void RemoveEventHandler(TEventCode code)
        {
            EventHandlerRegister.RemoveHandler(code);
        }
    }
}