using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace UserService.DataAccess
{
    public class MySQLHelper : BaseDatabaseHelper
    {
        private readonly DatabaseConfig databaseConfig;

        public MySQLHelper(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public override IDbConnection GetConnection()
        {
            return new MySqlConnection(databaseConfig.ConnectionString);
        }
    }
}
