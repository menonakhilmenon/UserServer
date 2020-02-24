using System;

namespace UserService
{
    public class EventManager
    {
        public void InvokeEvent(string eventName, params object[] parameters)
        {

        }
    }
    public static class UserServiceEvents 
    {
        public static string CHARACTER_CHANGE_EVENT = "CharacterDataChanged";
        public static string CHARACTER_LOGIN_EVENT = "CharacterLoggedIn";
        public static string CHARACTER_LOGOUT_EVENT = "CharacterLoggedIn";

        public static string CHARACTER_CREATE_EVENT = "CharacterCreated";
        public static string CHARACTER_DELETE_EVENT = "CharacterDeleted";
        public static string USER_CREATE_EVENT = "UserCreated";
        public static string USER_DELETE_EVENT = "UserDeleted";

        public static string USER_CHANGE_EVENT = "UsernameChanged";

    }
}