
namespace Angular_auth_Api.Servicess
{
    public class HashPassword
    {
       public static string GenrateHash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        }

        public static bool MatchHash(string pass, string hpass) 
        { 
            return BCrypt.Net.BCrypt.EnhancedVerify(pass, hpass);
        }
    }
}
