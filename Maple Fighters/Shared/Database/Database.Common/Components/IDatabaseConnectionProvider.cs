using System.Data;
using ComponentModel.Common;

namespace Database.Common.Components
{
    public interface IDatabaseConnectionProvider : IExposableComponent
    {
        IDbConnection GetDbConnection();
    }
}