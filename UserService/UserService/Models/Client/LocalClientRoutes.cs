namespace UserService.Models.Client
{
    public static class LocalClientRoutes
    {
        //For local player login/logout
        public const string GET_CHARACTERS_ROUTE = "GetCharacters";
        public const string LOGIN_CHAR_ROUTE = "SelectCharacter";
        public const string LOGOUT_ROUTE = "LogoutCharacter";
        
        //For Adding/Removing characters
        public const string ADD_CHAR_ROUTE = "AddCharacter";
        public const string REMOVE_CHAR_ROUTE = "RemoveCharacter";

        //For Modifying Characters
        public const string MODIFY_CHAR_VISUAL_ROUTE = "ModifyCharacterVisual";
        public const string MODIFY_CHAR_NAME_ROUTE = "ModifyCharacterName";
        public const string MODIFY_USER_NAME_ROUTE = "ModifyUserName";
    }
}
