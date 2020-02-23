using System;

namespace UserService
{
    public class EventManager
    {
        public void InvokeEvent(string eventName, params object[] parameters)
        {

        }
    }
    public static class Events 
    {
        public static string CHARACTER_CHANGE_EVENT = "CharacterDataChanged";
        public static string CHARACTER_LOGIN_EVENT = "CharacterLoggedIn";
        public static string CHARACTER_LOGOUT_EVENT = "CharacterLoggedIn";
    }
}