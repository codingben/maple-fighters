using ComponentModel.Common;

namespace ServerApplication.Common.Components
{
    public interface IIdGenerator : IExposableComponent
    {
        int GenerateId();
    }
}