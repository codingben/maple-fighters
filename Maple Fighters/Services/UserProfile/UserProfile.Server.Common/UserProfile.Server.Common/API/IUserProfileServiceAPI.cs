using System;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using ComponentModel.Common;

namespace UserProfile.Server.Common
{
    public interface IUserProfileServiceAPI : IExposableComponent
    {
        event Action<UserProfilePropertiesChangedEventParameters> UserProfilePropertiesChanged;

        Task<CreateUserProfileResponseParameters> CreateUserProfile(IYield yield, CreateUserProfileRequestParameters parameters);
        void ChangeUserProfileProperties(ChangeUserProfilePropertiesRequestParameters parameters);
    }
}