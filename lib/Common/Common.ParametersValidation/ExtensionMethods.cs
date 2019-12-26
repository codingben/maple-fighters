using FluentValidation.Results;

namespace Common.ParametersValidation
{
    public static class ExtensionMethods
    {
        public static string ToErrorMessage(this ValidationResult validationResult)
        {
            var errorMessage = 
                !validationResult.IsValid ? validationResult.ToString() : null;
            return errorMessage;
        }
    }
}