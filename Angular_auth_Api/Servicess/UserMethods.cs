using Angular_auth_Api.Interfaces;
using Angular_auth_Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Angular_auth_Api.Servicess
{
    public class UserMethods : IUser
    {
        private readonly Context _context;

        public UserMethods(Context context)
        {
            _context = context;
        }

        public async Task<ActionResult<Users>> Authenticate(LoginUser userObj)
        {
            var UserStatus = await _context.UserTable.FirstOrDefaultAsync(x => 
            x.email == userObj.email);
            
           if (UserStatus != null && HashPassword.MatchHash(userObj.password, UserStatus.password) )
            {
                JWT jwt = new JWT();
                UserStatus.token = jwt.createJWTToken(UserStatus);
                return UserStatus;
            }
            else {
                return null;
            };
        }


        public async Task<bool> RegisterUser(Users userobj)
        {
            try
            {
                userobj.password = HashPassword.GenrateHash(userobj.password);
                userobj.role = "admin";
                await _context.UserTable.AddAsync(userobj);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CheckUsernameExists(string username)
        {
            return await _context.UserTable.AnyAsync(x => x.username == username);
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            return await _context.UserTable.AnyAsync(x => x.email == email);
        }

        public async Task<List<UserData>> GetUsers()
        {
            var usersData = await _context.UserTable
                                .Select(u => new UserData
                                {
                                    name = u.firstName + " " +  u.lastName,
                                    email = u.email,
                                    username = u.username,
                                    role = u.role
                                }).ToListAsync();
            return usersData;

        }
    }
}
