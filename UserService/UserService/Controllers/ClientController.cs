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
    public class ClientController : Controller
    {
        private readonly IGetCharacterData characterData;
        private readonly CharacterSessionManager activeCharacterManager;
        public ClientController(IGetCharacterData characterData, CharacterSessionManager activeCharacterManager)
        {
            this.characterData = characterData;
            this.activeCharacterManager = activeCharacterManager;
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
                var res = await activeCharacterManager.LoginCharacter(HttpContext.GetUserIDFromJWTHeader(),charID);
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
                    activeCharacterManager.LogoutUser(HttpContext.GetUserIDFromJWTHeader());
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
                activeCharacterManager.LogoutUser(HttpContext.GetUserIDFromJWTHeader());
                return Ok();
            }
            catch (Exception ie)
            {
                Console.WriteLine(ie.ToString());
                return BadRequest();
            }
        }
        [HttpPost]
        [Route(LocalClientRoutes.ADD_CHAR_ROUTE)]
        public IActionResult AddCharacter(CharacterFull character)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        [Route(LocalClientRoutes.REMOVE_CHAR_ROUTE)]
        public IActionResult RemoveCharacter(string charID)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        [Route(LocalClientRoutes.MODIFY_CHAR_NAME_ROUTE)]
        public IActionResult ChangeCharacterName(string charID, string name)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route(LocalClientRoutes.MODIFY_CHAR_VISUAL_ROUTE)]
        public IActionResult ChangeVisual(string charID,string characterVisualData)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        [Route(LocalClientRoutes.MODIFY_USER_NAME_ROUTE)]
        public IActionResult ChangeUserName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
