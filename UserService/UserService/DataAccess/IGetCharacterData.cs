using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.DataAccess
{
    public interface IGetCharacterData
    {
        Task<IEnumerable<CharacterFull>> GetUserCharacters(string userID);
        Task<CharacterFull> GetCharacterInfo(string charID);
        Task<User> GetUserInfo(string userID);
    }
}