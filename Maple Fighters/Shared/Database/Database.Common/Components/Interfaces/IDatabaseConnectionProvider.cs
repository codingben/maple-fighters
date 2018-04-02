using System.Data;

namespace Database.Common.Components.Interfaces
{
    public interface IDatabaseConnectionProvider
    {
        IDbConnection GetDbConnection();
    }
}