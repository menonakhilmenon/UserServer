using System;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.DataAccess
{
    public interface IDeleteCharacter
    {
        Task<bool> DeleteUser(string userID);
        Task<bool> DeleteCharacter(string characterID,User user);
    }
}
