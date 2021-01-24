using System;
using UnityEngine;

namespace Scripts.Services.AuthenticatorApi
{
    [Serializable]
    public class ErrorData
    {
        public string[] errorMessages;

        public static string FromJsonToErrorMessage(string json)
        {
            var errorData = JsonUtility.FromJson<ErrorData>(json);
            var errorMessages = errorData.errorMessages;
            var errorMessage = string.Empty;

            if (errorMessages.Length != 0)
            {
                // TODO: Make sure won't be more than one error message
                errorMessage = errorMessages[0];
            }

            return errorMessage;
        }
    }
}