using ComponentModel.Common;

namespace ServerApplication.Common.Components
{
    public interface IRandomNumberGenerator : IExposableComponent
    {
        int GenerateRandomNumber();
        int GenerateRandomNumber(int min, int max);
    }
}