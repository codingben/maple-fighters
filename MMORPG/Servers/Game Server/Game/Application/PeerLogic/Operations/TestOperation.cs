using System.Collections.Generic;
using Shared.Common.Communications;
using Shared.Game.Common;
using Shared.Servers.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class TestOperation : IOperation
    {
        private readonly EventSender eventSender;

        public TestOperation(EventSender eventSender)
        {
            this.eventSender = eventSender;
        }

        public Dictionary<byte, object> OperationHandler(Dictionary<byte, object> parameters)
        {
            var requestParameters = parameters.ToParameters<TestRequestParameters>();

            Logger.Log.Debug("TestOperation->Handle() = " + requestParameters.MagicNumber);

            eventSender.SendEvent((byte)GameEvents.Test, new TestParameters { MagicNumber = 18 });

            return new TestResponseParameters { MagicNumber = 10 }.ToDictionary();
        }
    }
}