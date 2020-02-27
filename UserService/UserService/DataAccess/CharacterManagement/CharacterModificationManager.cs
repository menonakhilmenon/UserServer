using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.DataAccess.CharacterManagement
{
    public class CharacterModificationManager
    {
        private readonly CharacterCache characterCache;
        private readonly EventManager eventManager;
        private readonly IGetCharacterData getCharacterData;
        private readonly ISetCharacterData setCharacterData;
        public CharacterModificationManager(CharacterCache characterCache, EventManager eventManager, IGetCharacterData getCharacterData, ISetCharacterData setCharacterData)
        {
            this.characterCache = characterCache;
            this.eventManager = eventManager;
            this.getCharacterData = getCharacterData;
            this.setCharacterData = setCharacterData;
        }

        public async Task<bool> ChangeCharacterName(string userID,string charID, string charName)
        {
            var character = await characterCache.GetCharacter(charID);
            if (character != null)
            {
                if(character.character.userID == userID) 
                {
                    character.CharacterName = charName;
                    eventManager.InvokeEvent(UserServiceEvents.CHARACTER_CHANGE_EVENT, charID);
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> ChangeCharacterVisual(string userID,string charID, JObject charVisual) 
        {
            var character = await characterCache.GetCharacter(charID);
            if (character != null) 
            {
                if(character.character.userID == userID) 
                {
                    var user = await getCharacterData.GetUserData(userID);
                    if (user != null) 
                    {
                        if(user.userData.characterAlterationLimit > 0) 
                        {
                            user.userData.characterAlterationLimit--;
                            var userRes = await setCharacterData.SetUserData(userID, user.userData);
                            if (userRes) 
                            {
                                character.visualData = charVisual;
                                eventManager.InvokeEvent(UserServiceEvents.CHARACTER_CHANGE_EVENT, charID);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public async Task<bool> ChangeUserName(string userID, string userName)
        {
            var res = await setCharacterData.SetUserName(userID, userName);
            if (res) 
            {
                eventManager.InvokeEvent(UserServiceEvents.USER_CHANGE_EVENT, userID);
            }
            return res;
        }
    }
}
