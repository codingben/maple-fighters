using System.Data;
using Database.Common.Configuration;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServiceStack.OrmLite;

namespace Database.Common.Components
{
    public class DatabaseConnectionProvider : Component<IServerEntity>
    {
        private readonly OrmLiteConnectionFactory dbFactory;

        public DatabaseConnectionProvider()
        {
            var connectionString = DatabaseConnectionConfiguraiton.GetConnectionString();
            dbFactory = new OrmLiteConnectionFactory(connectionString, PostgreSqlDialect.Provider);
        }

        public IDbConnection GetDbConnection()
        {
            return dbFactory.Open();
        }
    }
}