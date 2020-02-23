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
        public async Task<bool> CallStoredProcedureExec(string procedureName,DynamicParameters parameters)
        {
            using (var dbConnection = GetConnection())
            {
                var res = await dbConnection.ExecuteAsync(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return res > 0;
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
