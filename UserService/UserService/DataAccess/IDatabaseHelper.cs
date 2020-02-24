using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace UserService.DataAccess
{
    public interface IDatabaseHelper
    {
        Task<int> CallStoredProcedureExec(string procedureName, DynamicParameters parameters);
        Task<int> CallStoredProcedureExec(IDbConnection connection,string procedureName, DynamicParameters parameters);
        Task<IEnumerable<T>> CallStoredProcedureQuery<T>(string procedureName, DynamicParameters parameters);
        Task<IEnumerable<T>> CallStoredProcedureQuery<T>(IDbConnection connection,string procedureName, DynamicParameters parameters);
        public IDbConnection GetConnection();
    }
}