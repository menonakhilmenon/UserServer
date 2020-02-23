using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.DataAccess
{
    public class ActiveCharacterManager
    {
        private readonly IGetCharacterData characterDataGetter;
        private readonly ISetCharacterData characterDataSetter;
        private readonly Dictionary<string, CachedCharacter> characters;
        private readonly Dictionary<string, string> activeCharacters;

        public ActiveCharacterManager(IGetCharacterData characterDataGetter, ISetCharacterData characterDataSetter)
        {
            characters = new Dictionary<string, CachedCharacter>();
            activeCharacters = new Dictionary<string, string>();
            this.characterDataGetter = characterDataGetter;
            this.characterDataSetter = characterDataSetter;
        }


        public bool IsLoggedIn(string id) 
        {
            return activeCharacters.ContainsKey(id) || activeCharacters.ContainsValue(id);
        }

        public CachedCharacter GetActiveCharacter(string userID) 
        {
            if (activeCharacters.ContainsKey(userID)) 
            {
                var charID = activeCharacters[userID];
                if (characters.TryGetValue(charID,out var res))
                {
                    return res;
                }
                return null;
            }
            return null;
        }

        public async Task LoginCharacter(string charID)
        {
            var res = await GetCharacter(charID);
            var user = res.character.userID;
            if(activeCharacters.TryGetValue(user,out var character)) 
            {
                await LogoutCharacter(character);
            }
            activeCharacters[user] = charID;
        }

        public async Task<CachedCharacter> GetCharacter(string charID) 
        {
            if(characters.TryGetValue(charID,out var character)) 
            {
                return character;
            }
            else 
            {
                var res = await characterDataGetter.GetCharacterInfo(charID);
                return AddToCache(charID, res);
            }
        }

        public async Task LogoutCharacter(string charID) 
        {
            if(characters.TryGetValue(charID,out var res))
            {
                if(activeCharacters.TryGetValue(res.character.userID,out var activeCharID)) 
                {
                    if(activeCharID == charID) 
                    {
                        activeCharacters.Remove(res.character.userID);
                    }
                }
            }
            await EvictFromCache(charID);
        }

        private async Task EvictFromCache(string charID) 
        {
            if(characters.TryGetValue(charID,out var res)) 
            {
                if (res.dataChanged) 
                {
                    if(!await characterDataSetter.SetCharacterGameData(charID, res.characterGameData)) 
                    {
                        Console.WriteLine("ERROR WRITING TO DATABASE");
                        return;
                    }
                }
                if (res.nameChanged) 
                {
                    if (!await characterDataSetter.SetCharacterName(charID, res.CharacterName))
                    {
                        Console.WriteLine("ERROR WRITING TO DATABASE");
                        return;
                    }
                }
                if (!await characterDataSetter.SetCharacterVisualData(charID, res.visualData))
                {
                    Console.WriteLine("ERROR WRITING TO DATABASE");
                    return;
                }
                characters.Remove(charID);
            }
        }

        public async Task LogoutUser(string userID)
        {
            if (activeCharacters.TryGetValue(userID, out var charID))
            {
                await LogoutCharacter(charID);
                activeCharacters.Remove(userID);
            }
        }

        private CachedCharacter AddToCache(string charID,CharacterFull character) 
        {
            var res = new CachedCharacter(character);
            characters.TryAdd(charID, res);
            return res;
        }
    }
}
