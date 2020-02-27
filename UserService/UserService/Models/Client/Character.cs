using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{

    public class CharacterFull 
    {
        public string characterName { get; set; }
        public JObject characterVisualData { get; set; }
        public string characterID { get; set; }
        public CharacterGameData characterGameData { get; set; }
        public string userID { get; set; }
    }
    public class NewCharacter 
    {
        public string characterName { get; set; }
        public JObject characterVisualData { get; set; }
    }
}
