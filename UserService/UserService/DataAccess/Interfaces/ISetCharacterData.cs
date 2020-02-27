using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.DataAccess
{
    public interface ISetCharacterData
    {
        Task<bool> SetUserData(string userID, UserData gameData);
        Task<bool> SetCharacterVisualData(string userID, JObject visualData);
        Task<bool> SetCharacterGameData(string userID, CharacterGameData gameData);
        Task<bool> SetUserName(string userID, string name);
        Task<bool> SetCharacterName(string userID, string name);
    }
}
