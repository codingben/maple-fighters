using ComponentModel.Common;

namespace UserProfile.Service.Application.Components
{
    internal interface IDatabaseUserProfileCreator : IExposableComponent
    {
        void Create(int userId);
    }
}