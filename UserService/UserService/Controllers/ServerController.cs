using Microsoft.AspNetCore.Mvc;
using System;
using UserService.Models.Servers;

namespace UserService.Controllers
{
    public class ServerController : Controller
    {
        [HttpPost]
        [Route(ServerRoutes.SET_CHAR_INFO_ROUTE)]
        public IActionResult SetCharacterData()
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        [Route(ServerRoutes.SET_USER_INFO_ROUTE)]
        public IActionResult SetUserData()
        {
            throw new NotImplementedException();
        }
    }
}