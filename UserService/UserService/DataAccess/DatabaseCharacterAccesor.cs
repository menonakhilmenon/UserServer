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
            return await dbHelper.CallStoredProcedureExec("CreateCharacter",
                new DynamicParameters()
                .AddParameter("charID", character.characterID)
                .AddParameter("userID",character.userID)
                .AddParameter("charName",character.characterName)
                .AddParameter("charVisual",character.characterVisualData)
                .AddParameter("charData",JsonConvert.SerializeObject(character.characterGameData))
                ) > 0;

        }
        public async Task<bool> CreateUser(User user)
        {
            return await dbHelper.CallStoredProcedureExec("CreateUser",
                new DynamicParameters()
                .AddParameter("userID", user.userID)
                .AddParameter("userName",user.userName)
                .AddParameter("userData",JsonConvert.SerializeObject(user.userData))) > 0;
        }
        public async Task<IEnumerable<CharacterFull>> GetUserCharacters(string userID)
        {
            var res = await dbHelper.CallStoredProcedureQuery<CharacterFull>("GetUserCharacters",
                new DynamicParameters().AddParameter("userID", userID));
            foreach (var item in res)
            {
                item.userID = userID;
            }
            return res;
        }
        public async Task<User> GetUserData(string userID)
        {
            var res = (await dbHelper.CallStoredProcedureQuery<User>("GetUserData",
                new DynamicParameters()
                .AddParameter("userID", userID)))
                .FirstOrDefault();
            res.userID = userID;
            return res;
        }
        public async Task<CharacterFull> GetCharacterData(string charID)
        {
            var res = (await dbHelper.CallStoredProcedureQuery<CharacterFull>("GetCharacterData",
                new DynamicParameters()
                .AddParameter("charID", charID)))
                .FirstOrDefault();
            res.characterID = charID;
            return res;
        }
        public async Task<bool> SetUserData(string userID, UserGameData gameData)
        {
            return await dbHelper.CallStoredProcedureExec("SetUserData",new DynamicParameters()
                .AddParameter("userID", userID)
                .AddParameter("userData", JsonConvert.SerializeObject(gameData))) > 0;
        }
        public async Task<bool> SetCharacterVisualData(string charID, string visualData)
        {
            return await dbHelper.CallStoredProcedureExec("SetCharacterVisualData", new DynamicParameters()
                .AddParameter("charID", charID)
                .AddParameter("charVisualData", visualData)) > 0;
        }
        public async Task<bool> SetCharacterGameData(string charID, CharacterGameData gameData)
        {
            return await dbHelper.CallStoredProcedureExec("SetCharacterGameData", new DynamicParameters()
                .AddParameter("charID", charID)
                .AddParameter("charGameData", JsonConvert.SerializeObject(gameData))) > 0;
        }
        public async Task<bool> SetUserName(string userID, string name)
        {
            return await dbHelper.CallStoredProcedureExec("SetUserName", new DynamicParameters()
                .AddParameter("userID", userID)
                .AddParameter("userName", name)) > 0;
        }
        public async Task<bool> SetCharacterName(string charID, string name)
        {
            return await dbHelper.CallStoredProcedureExec("SetCharacterName", new DynamicParameters()
                .AddParameter("charID", charID)
                .AddParameter("charName", name)) > 0;
        }
    }
}
