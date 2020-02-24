using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.DataAccess
{
    public class CharacterCache
    {
        private const int CACHE_EVICTION_DELAY = 30000;
        private readonly IGetCharacterData characterDataGetter;
        private readonly ISetCharacterData characterDataSetter;
        private readonly Dictionary<string, CachedCharacter> characters;

        public CharacterCache(IGetCharacterData characterDataGetter, ISetCharacterData characterDataSetter)
        {
            characters = new Dictionary<string, CachedCharacter>();
            this.characterDataGetter = characterDataGetter;
            this.characterDataSetter = characterDataSetter;
        }

        public bool IsCharacterInCache(string charID) 
        {
            return characters.ContainsKey(charID);
        }

        public async Task<CachedCharacter> GetCharacter(string charID) 
        {
            if(characters.TryGetValue(charID,out var character)) 
            {
                return character;
            }
            else 
            {

                var res = AddToCache(charID, await characterDataGetter.GetCharacterData(charID));
                _ = EvictFromCache(charID);
                return res;
            }
        }

        
        private async Task EvictFromCache(string charID) 
        {
            if(characters.TryGetValue(charID,out var res)) 
            {
                await Task.Delay(CACHE_EVICTION_DELAY);
                while (!res.evict)
                {
                    await WriteToDB(res, charID);
                    await Task.Delay(CACHE_EVICTION_DELAY);
                }
                characters.Remove(charID);
            }
        }


        private async Task WriteToDB(CachedCharacter res,string charID) 
        {
            if (res.dataChanged)
            {
                if (!await characterDataSetter.SetCharacterGameData(charID, res.characterGameData))
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
            if (res.visualChanged)
            {
                if (!await characterDataSetter.SetCharacterVisualData(charID, res.visualData))
                {
                    Console.WriteLine("ERROR WRITING TO DATABASE");
                    return;
                }
            }
            if (res.valueAccessed && !res.valueChanged)
            {
                res.ResetEviction();
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
