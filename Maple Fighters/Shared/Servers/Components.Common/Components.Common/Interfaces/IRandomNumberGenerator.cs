namespace Components.Common.Interfaces
{
    public interface IRandomNumberGenerator
    {
        int GenerateRandomNumber();
        int GenerateRandomNumber(int min, int max);
    }
}