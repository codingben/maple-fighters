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
        private readonly Dictionary<byte, IOperationHandler> operationsContainer = new Dictionary<byte, IOperationHandler>();

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

            var responseParameters = operation.Handle(operationRequest.Parameters); // await?
            if (responseParameters == null)
            {
                var operationResponse = new OperationResponse(operationCode);
                SendOperationResponse(operationResponse, sendParameters);
            }
            else
            {
                var operationResponse = new OperationResponse(operationCode, responseParameters);
                SendOperationResponse(operationResponse, sendParameters);
            }
        }

        protected void SetOperationRequestHandler(GameOperations operationCode, IOperationHandler operation)
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

    /// <summary>
    /// Every data (property) on parameters structure should use this attribute in order to be able to transfer this data over the network.
    /// </summary>
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
        public static Dictionary<byte, object> Serialize<T>(T parameters)
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

    /// <summary>
    /// Every structure that handles request or response parameters should inherit from this interface.
    /// </summary>
    public interface IParameters
    {
        // Left blank intentionally
    }

    /// <summary>
    /// An interface which a handler class of an operation should implement.
    /// </summary>
    internal interface IOperationHandler
    {
        /// <summary>
        /// Handle() is getting a request parameters and giving a response parameters.
        /// The following type is: (ResponseParameters : IParameters) Handle((RequestParameters : IParameters))
        /// </summary>
        /// <returns>It should return response parameters or null, which meaning that there are no parameters to send back.</returns>
        Dictionary<byte, object> Handle(Dictionary<byte, object> parameters);
    }

    internal class TestOperation : IOperationHandler
    {
        public Dictionary<byte, object> Handle(Dictionary<byte, object> parameters)
        {
            var requestParameters = (TestRequestParameters)ParametersSerializer.Dserialize<TestRequestParameters>(parameters);

            Logger.Log.Debug("TestOperation->Handle() = " + requestParameters.MagicNumber);

            return ParametersSerializer.Serialize(new TestResponseParameters { MagicNumber = 10 });
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