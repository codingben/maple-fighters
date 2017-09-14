using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;

namespace Scripts.Services
{
    public static class ExtensionMethods
    {
        public static async Task<TParam> ProvideSubscription<TParam>(this IOperationResponseSubscriptionProvider subscriptionProvider, IYield yield, short requestId)
            where TParam : struct, IParameters
        {
            var receiver = new SafeOperationResponseReceiver<TParam>(Configuration.OperationTimeout);
            subscriptionProvider.ProvideSubscription(receiver, requestId);

            await yield.Return(receiver);

            if (receiver.HasException)
            {
                throw new OperationNotHandledException(receiver.ExceptionReturnCode);
            }

            return receiver.Value;
        }

        public static async Task ProvideSubscription(this IOperationResponseSubscriptionProvider subscriptionProvider, IYield yield, short requestId)
        {
            var receiver = new SafeOperationResponseReceiver<EmptyParameters>(Configuration.OperationTimeout);
            subscriptionProvider.ProvideSubscription(receiver, requestId);

            await yield.Return(receiver);

            if (receiver.HasException)
            {
                throw new OperationNotHandledException(receiver.ExceptionReturnCode);
            }
        }
    }
}