using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class CharacterClient:CharacterBase
    {
        public string characterName { get; set; }
        public string characterVisualData { get; set; }
    }

    public class CharacterBase 
    {
        public string characterID { get; set; }
        //public string _characterGameData { get; set; }
        public CharacterGameData characterGameData { get; set; }
    }
    public class CharacterFull : CharacterClient 
    {
        public string userID { get; set; }
    }
}
