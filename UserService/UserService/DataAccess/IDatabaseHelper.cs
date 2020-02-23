using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace UserService.DataAccess
{
    public interface IDatabaseHelper
    {
        Task<bool> CallStoredProcedureExec(string procedureName, DynamicParameters parameters);
        Task<IEnumerable<T>> CallStoredProcedureQuery<T>(string procedureName, DynamicParameters parameters);
        public IDbConnection GetConnection();
    }
}