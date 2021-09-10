using UnityEngine;

namespace Scripts.Services.AuthenticatorApi
{
    public struct ErrorData
    {
        public string[] errorMessages;

        public static string FromJsonToErrorMessage(string json)
        {
            var errorData = JsonUtility.FromJson<ErrorData>(json);
            var errorMessages = errorData.errorMessages;
            var errorMessage = string.Empty;

            if (errorMessages.Length != 0)
            {
                errorMessage = errorMessages[0];
            }

            return errorMessage;
        }
    }
}