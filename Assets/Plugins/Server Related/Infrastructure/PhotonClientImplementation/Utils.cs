using System.Collections.Generic;
using CommonCommunicationInterfaces;
using ExitGames.Client.Photon;
using PhotonCommonImplementation;
using UnityEngine;

namespace PhotonClientImplementation
{
	// @author amit115532 (Amit Ozalvo)
    public static class Utils
    {
        public static OperationRequest ToPhotonOperationRequest<TParam>(MessageData<TParam> request, short requestId) 
            where TParam : struct, IParameters
        {
            var photonParameters = PhotonCommonImplementation.Utils.ToPhotonParameters(request.Parameters);
            var operationRequest = new OperationRequest
            {
                OperationCode = request.Code,
                Parameters = photonParameters
            };

            operationRequest.SetRequestId(requestId);

            return operationRequest;
        }

        public static OperationResponse ToPhotonOperationResponse(RawMessageResponseData response, short requestId)
        {
            var photonParameters = PhotonCommonImplementation.Utils.ToPhotonParameters(response.Parameters);
            var operationResponse = new OperationResponse
            {
                OperationCode = response.Code,
                Parameters = photonParameters,
                ReturnCode = response.ReturnCode
            };

            operationResponse.SetRequestId(requestId);

            return operationResponse;
        }

        public static void SetRequestId(this OperationRequest operationRequest, short requestId)
        {
            if (operationRequest.Parameters == null)
            {
                operationRequest.Parameters = new Dictionary<byte, object>();
            }

            PhotonCommonImplementation.Utils.SetRequestId(operationRequest.Parameters, requestId);
        }

        public static void SetRequestId(this OperationResponse operationResponse, short requestId)
        {
            if (operationResponse.Parameters == null)
            {
                operationResponse.Parameters = new Dictionary<byte, object>();
            }

            PhotonCommonImplementation.Utils.SetRequestId(operationResponse.Parameters, requestId);
        }

        public static short ExtractRequestId(this OperationRequest operationRequest)
        {
            return PhotonCommonImplementation.Utils.ExtractRequestId(operationRequest.Parameters);
        }

        public static short ExtractRequestId(this OperationResponse operationResponse)
        {
            return ExtractRequestId(operationResponse.Parameters);
        }

        private static short ExtractRequestId(IReadOnlyDictionary<byte, object> parameters)
        {
            var parameterCodeValue = GetAdditionalParameterCodeValue(AdditionalParameterCode.RequestId);

            Debug.Assert(parameters.ContainsKey(parameterCodeValue), "ExtractRequestId() -> Could not find requestId");

            return (short)parameters[parameterCodeValue];
        }

        private static byte GetAdditionalParameterCodeValue(AdditionalParameterCode parameterCode)
        {
            return (byte)(byte.MaxValue - parameterCode);
        }
    }
}