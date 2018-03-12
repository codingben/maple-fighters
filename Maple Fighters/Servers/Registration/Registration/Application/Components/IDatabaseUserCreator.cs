using ComponentModel.Common;

namespace Registration.Application.Components
{
    internal interface IDatabaseUserCreator : IExposableComponent
    {
        void Create(string email, string password, string firstName, string lastName);
    }
}