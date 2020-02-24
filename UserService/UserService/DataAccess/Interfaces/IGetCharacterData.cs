using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.DataAccess
{
    public interface IGetCharacterData
    {
        Task<CharacterFull> GetCharacterData(string charID);
        Task<CharacterFull> GetCharacterByName(string charName);
        Task<User> GetUserData(string userID);
        Task<User> GetUserByName(string userName);
        Task<IEnumerable<CharacterFull>> GetUserCharacters(string userID);
    }
}