using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.DataAccess.CharacterManagement
{
    public class CharacterModerationManager
    {
        private readonly CharacterCache activeCharacterManager;
        private readonly EventManager eventManager;
        public CharacterModerationManager(CharacterCache activeCharacterManager, EventManager eventManager)
        {
            this.activeCharacterManager = activeCharacterManager;
            this.eventManager = eventManager;
        }

        public async Task ChangeCharacterName(string charID,string charName) 
        {
            var character = await activeCharacterManager.GetCharacter(charID);
            character.CharacterName = charName;
            eventManager.InvokeEvent(Events.CHARACTER_CHANGE_EVENT, charID);
        }

    }
}
