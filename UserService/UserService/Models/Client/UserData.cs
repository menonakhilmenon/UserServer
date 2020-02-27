using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class UserData
    {
        public int characterCreationLimit { get; set; } = 3;
        public int characterAlterationLimit { get; set; } = 1;
    }
}
