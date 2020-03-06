using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class CharacterLoginRequest
    {
        public string charID { get; set; }
        public CharacterLoginRequest(string charID)
        {
            this.charID = charID;
        }
    }
}
