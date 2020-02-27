using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using UserService.DataAccess.CharacterManagement;
using UserService.Models;

namespace UserService.Controllers
{
    [Authorize(AuthenticationSchemes = JwtExtensionsAndConstants.JwtAuthenticationScheme)]
    public class CharacterModificationController : Controller
    {
        private readonly CharacterModificationManager characterModificationManager;

        public CharacterModificationController(CharacterModificationManager characterModificationManager)
        {
            this.characterModificationManager = characterModificationManager;
        }

        [HttpPost]
        [Route(UserServiceRoutes.MODIFY_CHAR_NAME_ROUTE)]
        public async Task<IActionResult> ChangeCharacterName(string charID, string name)
        {
            var res = await characterModificationManager.ChangeCharacterName(HttpContext.GetUserIDFromJWTHeader(),charID, name);
            return Ok(res);
        }

        [HttpPost]
        [Route(UserServiceRoutes.MODIFY_CHAR_VISUAL_ROUTE)]
        public async Task<IActionResult> ChangeVisual(string charID, JObject characterVisualData)
        {
            return Ok(await characterModificationManager.ChangeCharacterVisual(HttpContext.GetUserIDFromJWTHeader(), charID, characterVisualData));
        }
        [HttpPost]
        [Route(UserServiceRoutes.MODIFY_USER_NAME_ROUTE)]
        public async Task<IActionResult> ChangeUserName(string name)
        {
            return Ok(await characterModificationManager.ChangeUserName(HttpContext.GetUserIDFromJWTHeader(), name));
        }


    }
}