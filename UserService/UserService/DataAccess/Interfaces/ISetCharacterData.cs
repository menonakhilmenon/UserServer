using System;
using System.Threading.Tasks;
using UserService.Models;
using UserService.Models.Client;

namespace UserService.DataAccess
{
    public interface ISetCharacterData
    {
        Task<bool> SetUserData(string userID, UserData gameData);
        Task<bool> SetCharacterVisualData(string userID, string gameData);
        Task<bool> SetCharacterGameData(string userID, CharacterGameData gameData);
        Task<bool> SetUserName(string userID, string name);
        Task<bool> SetCharacterName(string userID, string name);
    }
}
