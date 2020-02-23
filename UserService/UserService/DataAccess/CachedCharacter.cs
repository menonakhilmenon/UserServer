using UserService.Models;

namespace UserService.DataAccess
{
    public class CachedCharacter
    {
        public CharacterFull _character = null;
        public bool nameChanged { get; private set; }
        public bool visualChanged { get; private set; }
        public bool dataChanged { get; private set; }

        public CachedCharacter(CharacterFull character)
        {
            _character = character;
            nameChanged = false;
            visualChanged = false;
            dataChanged = false;
            valueAccessed = false;
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
        public bool evict => !(dataChanged || nameChanged || visualChanged || valueAccessed);
        public bool valueAccessed { get; private set; }
        public bool valueChanged => dataChanged || nameChanged || visualChanged;
        public CharacterFull character 
        {
            get 
            {
                valueAccessed = true;
                return _character;
            } 
        }
        public void ResetEviction() 
        {
            valueAccessed = false;
            dataChanged = false;
            nameChanged = false;
            visualChanged = false;
        }
    }
}
