using System;
using System.Threading.Tasks;

namespace UserService.DataAccess
{
    public interface IDeleteCharacter
    {
        Task<bool> DeleteUser(string userID);
        Task<bool> DeleteCharacter(string characterID);
    }
}
