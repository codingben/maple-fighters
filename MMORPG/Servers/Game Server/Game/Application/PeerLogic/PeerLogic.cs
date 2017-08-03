using System;
using System.Collections.Generic;
using Game.Common;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace Game.Application.PeerLogic
{
    internal class PeerLogic : ClientPeerLogicBase
    {
        public PeerLogic(InitRequest initRequest) 
            : base(initRequest)
        {
            Logger.Log.Debug("A new peer connected!");

            AddOperations();
        }

        private void AddOperations()
        {
            SetOperationRequestHandler(GameOperations.Test, new TestOperation());
        }

        protected override void OnPeerDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            Logger.Log.Debug("A peer is disconnected.");
        }
    }

    internal abstract class ClientPeerLogicBase : ClientPeer
    {
        private readonly Dictionary<byte, IOperation> operationsContainer = new Dictionary<byte, IOperation>();

        protected ClientPeerLogicBase(InitRequest initRequest) 
            : base(initRequest)
        {
            // Left blank intentionally
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var operationCode = operationRequest.OperationCode;

            var operationHandler = operationsContainer.TryGetValue(operationRequest.OperationCode, out var operation);
            if (!operationHandler)
            {
                Logger.Log.Debug("Invalid operation requested!");
                return;
            }

            var requestParameters = operationRequest.Parameters;
            var handlerParameters = operation.Handle(requestParameters); // await?

            // TODO: Fix
            /*if (handlerParameters == null)
            {
                var operationResponse = new OperationResponse(operationCode);
                SendOperationResponse(operationResponse, sendParameters);
            }
            else
            {
                var parameters = ParametersSerializer.Serialize(ref handlerParameters);
                var operationResponse = new OperationResponse(operationCode);
                SendOperationResponse(operationResponse, sendParameters);
            }

            var operationResponse = response == null ? new OperationResponse(operationCode) 
                : new OperationResponse(operationCode, response);*/

            // SendOperationResponse(operationResponse, sendParameters);
        }

        protected void SetOperationRequestHandler(GameOperations operationCode, IOperation operation)
        {
            operationsContainer.Add((byte)operationCode, operation);
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            operationsContainer.Clear();

            OnPeerDisconnect(reasonCode, reasonDetail);
        }

        protected abstract void OnPeerDisconnect(DisconnectReason reasonCode, string reasonDetail);
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ParameterAttribute : Attribute
    {
        public byte Code { get; set; }
    }

    /// <summary>
    /// A serializer of the Photon's dictionary parameters.
    /// </summary>
    public static class ParametersSerializer
    {
        /// <summary>
        /// Serialize a parameters structure into a Dictionary to be able to transfer it through a network.
        /// </summary>
        /// <typeparam name="T">Should include the only Struct which inherits from IParameters interface.</typeparam>
        /// <param name="parameters">The parameters structure that we want to turn into Dictionary.</param>
        public static Dictionary<byte, object> Serialize<T>(ref T parameters)
            where T : struct, IParameters
        {
            var temp = new Dictionary<byte, object>();
            var type = parameters.GetType();

            foreach (var propertyInfo in type.GetProperties())
            {
                var properties = propertyInfo.GetCustomAttributes(typeof(ParameterAttribute), false) as ParameterAttribute[];
                if (properties == null)
                {
                    continue;
                }

                var codeParameter = properties[0].Code;
                temp.Add(codeParameter, propertyInfo.GetValue(parameters));
            }

            return temp;
        }

        /// <summary>
        /// Deserialize a Dictionary into a parameters structure to be able to read data which we have got over the network.
        /// </summary>
        /// <typeparam name="T">Should include the only Struct which inherits from IParameters interface.</typeparam>
        /// <param name="parameters">The parameters Dictionary that we want to turn into a structure.</param>
        public static object Dserialize<T>(Dictionary<byte, object> parameters)
            where T : struct, IParameters
        {
            object Object = new T();

            foreach (var propertyInfo in Object.GetType().GetProperties())
            {
                var properties = propertyInfo.GetCustomAttributes(typeof(ParameterAttribute), false) as ParameterAttribute[];
                if (properties == null)
                {
                    continue;
                }

                var codeParameter = properties[0].Code;
                var newTypeValue = Convert.ChangeType(parameters[codeParameter], propertyInfo.PropertyType);

                propertyInfo.SetValue(Object, newTypeValue);
            }

            return Object;
        }
    }

    public interface IParameters
    {
        // Left blank intentionally
    }

    internal interface IOperation
    {
        IParameters Handle(Dictionary<byte, object> requestParameters);
    }

    internal class TestOperation : IOperation
    {
        public IParameters Handle(Dictionary<byte, object> requestParameters)
        {
            var parameters = (TestRequestParameters)ParametersSerializer.Dserialize<TestRequestParameters>(requestParameters);

            Logger.Log.Debug("TestOperation->Handle() = " + parameters.MagicNumber);

            return new TestResponseParameters { MagicNumber = 10 };
        }
    }

    internal struct TestRequestParameters : IParameters
    {
        [Parameter(Code = 1)]
        public int MagicNumber { get; set; }
    }

    internal struct TestResponseParameters : IParameters
    {
        [Parameter(Code = 1)]
        public int MagicNumber { get; set; }
    }
}