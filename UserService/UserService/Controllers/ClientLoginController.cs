using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.DataAccess;
using JwtHelpers;
using UserService.Models.Client;
using UserService.Models;
using UserService.DataAccess.CharacterManagement;

namespace UserService.Controllers
{
    [Authorize(AuthenticationSchemes = JwtExtensionsAndConstants.JwtAuthenticationScheme)]
    public class ClientLoginController : Controller
    {
        private readonly IGetCharacterData characterData;
        private readonly CharacterSessionManager characterSessionManager;
        public ClientLoginController(IGetCharacterData characterData, CharacterSessionManager characterSessionManager)
        {
            this.characterData = characterData;
            this.characterSessionManager = characterSessionManager;
        }

        [HttpPost]
        [Route(LocalClientRoutes.GET_CHARACTERS_ROUTE)]
        public async Task<IActionResult> GetCharacters()
        {
            try
            {
                var res = await characterData.GetUserCharacters(HttpContext.GetUserIDFromJWTHeader());
                return new JsonResult(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        [Route(LocalClientRoutes.LOGIN_CHAR_ROUTE)]
        public async Task<IActionResult> SelectCharacter(string charID)
        {
            try 
            {
                var res = await characterSessionManager.LoginCharacter(HttpContext.GetUserIDFromJWTHeader(),charID);
                if (res) 
                {
                    return Ok();
                }
                else 
                {
                    return Unauthorized();
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.ToString());
                try 
                {
                    characterSessionManager.LogoutUser(HttpContext.GetUserIDFromJWTHeader());
                }
                catch (Exception ie)
                {
                    Console.WriteLine(ie.ToString());
                }
                return BadRequest();
            }
        }

        [HttpPost]
        [Route(LocalClientRoutes.LOGOUT_ROUTE)]
        public IActionResult LogoutCharacter()
        {
            try
            {
                characterSessionManager.LogoutUser(HttpContext.GetUserIDFromJWTHeader());
                return Ok();
            }
            catch (Exception ie)
            {
                Console.WriteLine(ie.ToString());
                return BadRequest();
            }
        }
    }
}
