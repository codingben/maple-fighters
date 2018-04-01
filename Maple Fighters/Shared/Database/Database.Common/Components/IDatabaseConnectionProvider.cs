using System.Data;

namespace Database.Common.Components
{
    public interface IDatabaseConnectionProvider
    {
        IDbConnection GetDbConnection();
    }
}