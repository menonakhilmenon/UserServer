using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.DataAccess.CharacterManagement
{
    public class CharacterModerationManager
    {
        private readonly EventManager eventManager;
        private readonly IGetCharacterData getCharacterData;
        private readonly ICreateCharacter createCharacter;
        private readonly IDeleteCharacter deleteCharacter;
        private readonly CharacterSessionManager characterSessionManager;
        public CharacterModerationManager(EventManager eventManager, IGetCharacterData getCharacterData, ICreateCharacter createCharacter, CharacterSessionManager characterSessionManager, IDeleteCharacter deleteCharacter)
        {
            this.eventManager = eventManager;
            this.getCharacterData = getCharacterData;
            this.createCharacter = createCharacter;
            this.characterSessionManager = characterSessionManager;
            this.deleteCharacter = deleteCharacter;
        }


        public async Task<bool> CreateCharacter(string userID,string charName,JObject visualData)
        {
            var user = await getCharacterData.GetUserData(userID);
            if(user == null) 
            {
                return false;
            }
            var oldChar = await getCharacterData.GetCharacterByName(charName);
            if (oldChar != null) 
            {
                return false;
            }
            if(user.userData.characterCreationLimit > 0) 
            {
                var guid = Guid.NewGuid().ToString();
                var newChar = new CharacterFull
                {
                    characterVisualData = visualData,
                    userID = userID,
                    characterName = charName,
                    characterID = guid
                };
                user.userData.characterCreationLimit--;
                var res = await createCharacter.CreateCharacter(newChar,user);

                if (res) 
                {
                    eventManager.InvokeEvent(UserServiceEvents.CHARACTER_CREATE_EVENT, guid, userID);
                }
                else 
                {
                    user.userData.characterCreationLimit++;
                }
                return res;
            }

            return false;
        }

        public async Task<bool> RemoveCharacter(string userID, string charID)
        {
            var res = await getCharacterData.GetCharacterData(charID);
            if(res.userID == userID) 
            {
                characterSessionManager.LogoutUser(userID);
                var user = await getCharacterData.GetUserData(res.userID);
                if (user != null) 
                {
                    user.userData.characterCreationLimit++;
                    var delRes = await deleteCharacter.DeleteCharacter(charID, user);
                    if (!delRes) 
                    {
                        user.userData.characterCreationLimit--;
                    }
                    else 
                    {
                        eventManager.InvokeEvent(UserServiceEvents.CHARACTER_DELETE_EVENT, charID, userID);
                    }
                    return delRes;
                }
                return false;
            }
            else 
            {
                return false;
            }
        }
        public async Task<bool> RemoveUser(string userID)
        {
            characterSessionManager.LogoutUser(userID);
            return (await deleteCharacter.DeleteUser(userID));
        }

        public async Task<bool> CreateUser(string userID, string name)
        {
            var user = await getCharacterData.GetUserData(userID);
            if (user != null)
            {
                return false;
            }
            user = await getCharacterData.GetUserByName(name);
            if (user != null)
            {
                return false;
            }
            return await createCharacter.CreateUser(
                new User 
                { 
                    userData = new UserData(),
                    userID = userID,
                    userName = name
                });
        }
    }
}
