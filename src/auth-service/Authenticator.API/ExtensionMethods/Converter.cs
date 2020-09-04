using System.Collections.Generic;
using FluentValidation.Results;

namespace Authenticator.API
{
    public static class Converter
    {
        public static string[] ConvertToErrorMessages(this IList<ValidationFailure> validationFailures)
        {
            var length = validationFailures.Count;
            var errorMessages = new string[length];

            for (var i = 0; i < length; i++)
            {
                errorMessages[i] = validationFailures[i].ErrorMessage;
            }

            return errorMessages;
        }
    }
}