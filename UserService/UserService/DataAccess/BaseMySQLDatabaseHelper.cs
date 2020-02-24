using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.DataAccess
{
    public sealed class FunctionParameter
    {
        public string name { get; set; }
        public object value { get; set; }
        public FunctionParameter(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
    }
    public abstract class BaseMySQLDatabaseHelper : IDatabaseHelper
    {
        public async Task<int> CallStoredProcedureExec(string procedureName, DynamicParameters parameters)
        {
            using (var dbConnection = GetConnection())
            {
                try
                {
                    return await dbConnection.ExecuteAsync(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.ToString());
                    return 0;
                }
            }
        }

        public async Task<int> CallStoredProcedureExec(IDbConnection dbConnection, string procedureName, DynamicParameters parameters)
        {
            try
            {
                return await dbConnection.ExecuteAsync(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

        public async Task<IEnumerable<T>> CallStoredProcedureQuery<T>(string procedureName, DynamicParameters parameters)
        {
            try
            {
                using (var dbConnection = GetConnection())
                {
                    var res = (await SqlMapper.QueryAsync<T>(dbConnection, procedureName, parameters, commandType: CommandType.StoredProcedure));
                    return res;
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }

        }

        public async Task<IEnumerable<T>> CallStoredProcedureQuery<T>(IDbConnection connection, string procedureName, DynamicParameters parameters)
        {
            try
            {
                var res = (await SqlMapper.QueryAsync<T>(connection, procedureName, parameters, commandType: CommandType.StoredProcedure));
                return res;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }

        }

        public abstract IDbConnection GetConnection();
    }
}
