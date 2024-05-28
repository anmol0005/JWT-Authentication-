using Angular_auth_Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Angular_auth_Api.Models;
using Angular_auth_Api.Servicess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Angular_auth_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _iuser;
        public UserController(IUser user, Context context)
        {
            _iuser = user;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateLogin([FromBody] LoginUser userObj)
        {
            if (userObj == null)
                return BadRequest();
            if (!await _iuser.CheckEmailExists(userObj.email))
            {
                return BadRequest(new { Message = "User Not Found" });
            }
            var checkUserStatus = await _iuser.Authenticate(userObj);
            if (checkUserStatus == null)
                return BadRequest(new { Message = "Password Incorrect" });

            return Ok(new
            {
                Token = checkUserStatus.Value.token,
                Message = "Login Success"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] Users userobj)
        {
            try
            {
                if (userobj == null)
                {
                    return BadRequest();
                }
                //Check username
                if (await _iuser.CheckUsernameExists(userobj.username))
                {
                    return BadRequest(new { Message = "username already exists" });
                }

                //Check username
                if (await _iuser.CheckEmailExists(userobj.email))
                {
                    return BadRequest(new { Message = "Email already exists" });
                }

                var regstatus = await _iuser.RegisterUser(userobj);
                if (regstatus)
                {
                    return Ok(new { Message = "User Registered Successfully" });
                }
                return BadRequest(new { Message = "Could not register, Try Again!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<Users>> GetUsers()
        {
            return Ok(await _iuser.GetUsers());
        }

    }
}
