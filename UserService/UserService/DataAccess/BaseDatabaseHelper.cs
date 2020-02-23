using Dapper;
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
    public abstract class BaseDatabaseHelper:IDatabaseHelper
    {
        public async Task<int> CallStoredProcedureExec(string procedureName,DynamicParameters parameters)
        {
            using (var dbConnection = GetConnection())
            {
                return await dbConnection.ExecuteAsync(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<T>> CallStoredProcedureQuery<T>(string procedureName, DynamicParameters parameters)
        {
            using (var dbConnection = GetConnection())
            {
                var res = (await SqlMapper.QueryAsync<T>(GetConnection(),procedureName, parameters, commandType: CommandType.StoredProcedure));
                return res;
            }
        }


        public abstract IDbConnection GetConnection();
    }
}
