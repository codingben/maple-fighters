using FluentValidation.Results;

namespace Common.ParametersValidation
{
    public static class ExtensionMethods
    {
        public static string ToErrorMessage(this ValidationResult validationResult)
        {
            return !validationResult.IsValid ? validationResult.ToString() : null;
        }
    }
}