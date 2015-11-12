using TagKid.Framework.Exceptions;

namespace Monospad.Core.Exceptions
{
    public static class Errors
    {
        public static readonly Error User_EmailAlreadyExists = new Error(1, "email already exists!");
        public static readonly Error User_InvalidEmailOrPassword = new Error(2, "invalid email or password!");
        public static readonly Error User_TokenExpired = new Error(3, "token expired!");
        public static readonly Error User_LoginNotFound = new Error(4, "login not found!");
        public static readonly Error User_UnregisteredEmailAddress = new Error(5, "email address not found in system!");
        
        public static readonly Error Auth_LoginRequired = new Error(20, "login required for this operation!");
        public static readonly Error Auth_LoginTokenExpired = new Error(21, "login token expired!");
        
        public static readonly Error Validation_InvalidEmailAddress = new Error(100, "invalid email address!");
        public static readonly Error Validation_InvalidPassword = new Error(101, "password must be 4-20 characters long!");
    }
}
