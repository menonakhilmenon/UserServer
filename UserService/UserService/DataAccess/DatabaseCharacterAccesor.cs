using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;
using Dapper;
using System.Text;
using Newtonsoft.Json.Linq;

namespace UserService.DataAccess
{
    public class DatabaseCharacterAccesor : IGetCharacterData, ISetCharacterData,ICreateCharacter,IDeleteCharacter
    {

        private readonly IDatabaseHelper dbHelper;

        public DatabaseCharacterAccesor(IDatabaseHelper dbHelper)
        {
            this.dbHelper = dbHelper;
        }
        public async Task<bool> CreateCharacter(CharacterFull character,User user)
        {
            using (var connection = dbHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var res1 = await dbHelper.CallStoredProcedureExec("CreateCharacter",
                        new DynamicParameters()
                        .AddParameter("charID", character.characterID)
                        .AddParameter("userID", character.userID)
                        .AddParameter("charName", character.characterName)
                        .AddParameter("charVisual", character.characterVisualData.ToString(Formatting.None))
                        .AddParameter("charData", JsonConvert.SerializeObject(character.characterGameData))
                        ) > 0;
                    if (!res1)
                    {
                        transaction.Rollback();
                        connection.Close();
                        return false;
                    }
                    var res2 = await SetUserData(user.userID, user.userData);
                    if (!res2)
                    {
                        transaction.Rollback();
                        connection.Close();
                        return false;
                    }
                    transaction.Commit();
                    connection.Close();
                    return true;
                }
            }

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
            if (res != null) 
            {
                res.userID = userID;
            }
            return res;
        }
        public async Task<CharacterFull> GetCharacterData(string charID)
        {
            var res = (await dbHelper.CallStoredProcedureQuery<CharacterFull>("GetCharacterData",
                new DynamicParameters()
                .AddParameter("charID", charID)))
                .FirstOrDefault();
            if (res != null) 
            {
                res.characterID = charID;
            }
            return res;
        }
        public async Task<bool> SetUserData(string userID, UserData gameData)
        {
            return await dbHelper.CallStoredProcedureExec("SetUserData",new DynamicParameters()
                .AddParameter("userID", userID)
                .AddParameter("userData", JsonConvert.SerializeObject(gameData))) > 0;
        }
        public async Task<bool> SetCharacterVisualData(string charID, JObject visualData)
        {
            return await dbHelper.CallStoredProcedureExec("SetCharacterVisualData", new DynamicParameters()
                .AddParameter("charID", charID)
                .AddParameter("charVisualData", visualData.ToString(Formatting.None))) > 0;
        }
        public async Task<bool> SetCharacterGameData(string charID, JObject gameData)
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

        public async Task<bool> DeleteUser(string userID)
        {
            return await dbHelper.CallStoredProcedureExec("DeleteUser", new DynamicParameters()
                .AddParameter("userID", userID)) > 0;

        }

        public async Task<bool> DeleteCharacter(string characterID, User user)
        {
            using (var connection = dbHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {

                    var res = await dbHelper.CallStoredProcedureExec(connection, "DeleteCharacter", new DynamicParameters()
                    .AddParameter("charID", characterID));
                    Console.WriteLine(res);
                    var res1 = res > 0;
                    if (!res1)
                    {
                        transaction.Rollback();
                        connection.Close();
                        return false;
                    }
                    var res2 = await SetUserData(user.userID, user.userData);
                    if (!res2)
                    {
                        transaction.Rollback();
                        connection.Close();
                        return false;
                    }
                    transaction.Commit();
                    connection.Close();
                    return true;
                }
            }
        }

        public async Task<CharacterFull> GetCharacterByName(string charName)
        {
            return (await dbHelper.CallStoredProcedureQuery<CharacterFull>("GetCharacterByName",
                new DynamicParameters()
                .AddParameter("charName", charName)))
                .FirstOrDefault();
        }

        public async Task<User> GetUserByName(string userName)
        {
            return (await dbHelper.CallStoredProcedureQuery<User>("GetUserByName",
                new DynamicParameters()
                .AddParameter("userName", userName)))
                .FirstOrDefault();
        }
    }
}
