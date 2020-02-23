using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models.Client;
using Dapper;
using UserService.Models;
using System.Text;

namespace UserService.DataAccess
{
    public class DatabaseCharacterAccesor : IGetCharacterData, ISetCharacterData
    {

        private readonly IDatabaseHelper dbHelper;

        public DatabaseCharacterAccesor(IDatabaseHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public async Task<bool> CreateCharacter(CharacterFull character)
        {
            return await dbHelper.CallStoredProcedureExec("InsertCharacter",
                new DynamicParameters().AddParameter("userID", JsonConvert.SerializeObject(character)));

        }
        public async Task<bool> CreateUser(User user)
        {
            return await dbHelper.CallStoredProcedureExec("InsertUser",
                new DynamicParameters().AddParameter("userID", JsonConvert.SerializeObject(user)));
        }
        public async Task<IEnumerable<CharacterFull>> GetUserCharacters(string userID)
        {
            var res = await dbHelper.CallStoredProcedureQuery<CharacterFull>("GetAllCharacterData",
                new DynamicParameters().AddParameter("userID", userID));
            foreach (var item in res)
            {
                item.userID = userID;
            }
            return res;
        }
        public async Task<User> GetUserInfo(string userID)
        {
            var res = (await dbHelper.CallStoredProcedureQuery<User>("GetUserData",
                new DynamicParameters().AddParameter("userID", userID)))
                .FirstOrDefault();
            res.userID = userID;
            return res;
        }
        public async Task<CharacterFull> GetCharacterInfo(string charID)
        {
            var res = (await dbHelper.CallStoredProcedureQuery<CharacterFull>("GetCharacterData",
                new DynamicParameters().AddParameter("userID", charID)))
                .FirstOrDefault();
            res.characterID = charID;
            return res;
        }
        public async Task<bool> SetUserData(string userID, UserGameData gameData)
        {
            return await dbHelper.CallStoredProcedureExec("SetUserData",new DynamicParameters()
                .AddParameter("userID", userID)
                .AddParameter("userID", JsonConvert.SerializeObject(gameData)));
        }
        public async Task<bool> SetCharacterVisualData(string userID, string visualData)
        {
            return await dbHelper.CallStoredProcedureExec("SetCharacterVisualData", new DynamicParameters()
                .AddParameter("userID", userID)
                .AddParameter("userID", JsonConvert.SerializeObject(visualData)));
        }
        public async Task<bool> SetCharacterGameData(string userID, CharacterGameData gameData)
        {
            return await dbHelper.CallStoredProcedureExec("SetCharacterGameData", new DynamicParameters()
                .AddParameter("userID", userID)
                .AddParameter("userID", JsonConvert.SerializeObject(gameData)));
        }
        public async Task<bool> SetUserName(string userID, string name)
        {
            return await dbHelper.CallStoredProcedureExec("SetUserName", new DynamicParameters()
                .AddParameter("userID", userID)
                .AddParameter("userID", name));
        }
        public async Task<bool> SetCharacterName(string userID, string name)
        {
            return await dbHelper.CallStoredProcedureExec("SetCharacterName", new DynamicParameters()
                .AddParameter("userID", userID)
                .AddParameter("userID", name));
        }
    }
}
