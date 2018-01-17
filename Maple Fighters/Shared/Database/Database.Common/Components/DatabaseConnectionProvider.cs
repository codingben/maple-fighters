using System.Data;
using System.Data.Common;
using ComponentModel.Common;
using Database.Common.Configuration;
using ServiceStack.OrmLite;

namespace Database.Common.Components
{
    public class DatabaseConnectionProvider : Component, IDatabaseConnectionProvider
    {
        private readonly OrmLiteConnectionFactory dbFactory;

        public DatabaseConnectionProvider()
        {
            DbColumn dbColumn;

            var connectionString = DatabaseConnectionConfiguraiton.GetConnectionString();
            dbFactory = new OrmLiteConnectionFactory(connectionString, MySqlDialect.Provider);
        }

        public IDbConnection GetDbConnection()
        {
            return dbFactory.Open();
        }
    }
}