using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.DataAccess
{
    public interface ICreateCharacter
    {
        Task<bool> CreateCharacter(CharacterFull character,User user);
        Task<bool> CreateUser(User character);
    }
}
