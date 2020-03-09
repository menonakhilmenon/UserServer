using Grpc.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.DataAccess;
using UserService.DataAccess.CharacterManagement;
using UserService.Models;
using UserService.Protos;
namespace UserService.Services
{
    public class UserManagementService: UserManagement.UserManagementBase
    {
        private readonly CharacterSessionManager characterSessionManager;
        private readonly CharacterCache characterCache;
        private readonly EventManager eventManager;

        public UserManagementService(CharacterSessionManager characterSessionManager, CharacterCache characterCache, EventManager eventManager)
        {
            this.characterSessionManager = characterSessionManager;
            this.characterCache = characterCache;
            this.eventManager = eventManager;
        }

        public override async Task<CharacterInfo> GetLoggedInUser(UserInfo request, ServerCallContext context)
        {
            var res = await characterSessionManager.GetActiveCharacter(request.UserID);
            if(res == null) 
            {
                return new CharacterInfo();
            }
            else 
            {
                return new CharacterInfo()
                {
                    CharData = JsonConvert.SerializeObject(res.characterGameData),
                    CharID = res.characterID
                };
            }
        }
        public override async Task<Empty> SaveUserData(CharacterInfo request, ServerCallContext context)
        {

            var cachedCharacter = await characterCache.GetCharacter(request.CharID);
            cachedCharacter.characterGameData = JsonConvert.DeserializeObject<JObject>(request.CharData);
            eventManager.InvokeEvent(UserServiceEvents.CHARACTER_CHANGE_EVENT, request.CharID);

            return new Empty();
        }
    }
}
