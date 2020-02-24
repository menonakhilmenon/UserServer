using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.DataAccess.CharacterManagement
{
    public class CharacterSessionManager
    {
        private readonly CharacterCache characterCache;
        private readonly Dictionary<string, string> activeCharacters;
        private readonly EventManager eventManager;

        public CharacterSessionManager(CharacterCache characterCache, EventManager eventManager)
        {
            this.characterCache = characterCache;
            activeCharacters = new Dictionary<string, string>();
            this.eventManager = eventManager;
        }

        public void LogoutUser(string userID)
        {
            if (activeCharacters.TryGetValue(userID, out var charID))
            {
                activeCharacters.Remove(userID);
                eventManager.InvokeEvent(UserServiceEvents.CHARACTER_LOGOUT_EVENT, charID);
            }
        }
        public bool IsUserLoggedIn(string userID)
        {
            return activeCharacters.ContainsKey(userID);
        }
        public bool IsCharacterLoggedIn(string charID)
        {
            return activeCharacters.ContainsValue(charID);
        }

        public string GetActiveCharacter(string userID)
        {
            if (activeCharacters.ContainsKey(userID))
            {
                return activeCharacters[userID];
            }
            return null;
        }

        public async Task<bool> LoginCharacter(string userID,string charID)
        {
            var res = await characterCache.GetCharacter(charID);
            if(res.character.userID != userID) 
            {
                return false;
            }
            eventManager.InvokeEvent(UserServiceEvents.CHARACTER_LOGIN_EVENT, charID);
            var user = res.character.userID;
            activeCharacters[user] = charID;
            return true;
        }

    }
}
