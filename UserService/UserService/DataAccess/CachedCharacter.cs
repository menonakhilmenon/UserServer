using UserService.Models;

namespace UserService.DataAccess
{
    public class CachedCharacter
    {
        public CharacterFull character = null;
        public bool nameChanged { get; set; }
        public bool visualChanged { get; set; }
        public bool dataChanged { get; set; }
        public bool evicting { get; set; }
        public CachedCharacter(CharacterFull character)
        {
            this.character = character;
            nameChanged = false;
            visualChanged = false;
            dataChanged = false;
        }

        public string visualData
        {
            get => character.characterVisualData;
            set
            {
                character.characterVisualData = value;
                visualChanged = true;
            }
        }
        public string CharacterName
        {
            get => character.characterName;
            set
            {
                character.characterName = value;
                nameChanged = true;
            }
        }
        public CharacterGameData characterGameData
        {
            get => character.characterGameData;
            set
            {
                character.characterGameData = value;
                dataChanged = true;
            }
        }
    }
}
