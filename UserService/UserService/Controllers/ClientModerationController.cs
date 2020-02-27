using System;
using System.Threading.Tasks;
using JwtHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.DataAccess.CharacterManagement;
using UserService.Models;

namespace UserService.Controllers
{
    [Authorize(AuthenticationSchemes = JwtExtensionsAndConstants.JwtAuthenticationScheme)]
    public class ClientModerationController : Controller
    {
        private readonly CharacterModerationManager characterModerationManager;

        public ClientModerationController(CharacterModerationManager characterModerationManager)
        {
            this.characterModerationManager = characterModerationManager;
        }

        [HttpPost]
        [Route(UserServiceRoutes.ADD_USER_ROUTE)]
        public async Task<IActionResult> CreateUser(string name)
        {
            try
            {
                return Ok(await characterModerationManager.CreateUser(HttpContext.GetUserIDFromJWTHeader(), name));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }
        }
        [HttpPost]
        [Route(UserServiceRoutes.ADD_CHAR_ROUTE)]
        public async Task<IActionResult> AddCharacter([FromBody]NewCharacter character)
        {
            try 
            {
                Console.WriteLine(character.characterVisualData.ToString(Newtonsoft.Json.Formatting.None));
                var res = await characterModerationManager.CreateCharacter(HttpContext.GetUserIDFromJWTHeader(), character.characterName, character.characterVisualData);
                return Ok(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }
        }
        [HttpPost]
        [Route(UserServiceRoutes.REMOVE_CHAR_ROUTE)]
        public async Task<IActionResult> RemoveCharacter(string charID)
        {
            try 
            {
                var res = await characterModerationManager.RemoveCharacter(HttpContext.GetUserIDFromJWTHeader(), charID);
                return Ok(res);
            }
            catch(Exception e) 
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }
        }
        [HttpPost]
        [Route(UserServiceRoutes.REMOVE_USER_ROUTE)]
        public async Task<IActionResult> RemoveUser(string userID)
        {
            try
            {
                var res = await characterModerationManager.RemoveUser(HttpContext.GetUserIDFromJWTHeader());
                return Ok(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }
        }

    }
}