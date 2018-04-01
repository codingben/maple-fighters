namespace ServerApplication.Common.Components
{
    public interface IRandomNumberGenerator
    {
        int GenerateRandomNumber();
        int GenerateRandomNumber(int min, int max);
    }
}