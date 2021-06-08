using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Smartfinance_server.Data;
using Smartfinance_server.Models;
using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Smartfinance_server.Controllers

//TODO comment out user endpoints that will not be needed, when production goes live

{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly QueryEngine _qe;

        public UserController(QueryEngine qe)
        {
            _qe = qe;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] Dictionary<string, JsonElement> user)
        {
            //TODO: recieve GOOGLE ID TOKEN

            // var user = _userService.Authenticate(model.Username, model.Password);

            // if (user == null)
            //     return BadRequest(new { message = "Username or password is incorrect" });

            // var tokenHandler = new JwtSecurityTokenHandler();
            // var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            // var tokenDescriptor = new SecurityTokenDescriptor
            // {
            //     Subject = new ClaimsIdentity(new Claim[]
            //     {
            //         new Claim(ClaimTypes.Name, user.Id.ToString())
            //     }),
            //     Expires = DateTime.UtcNow.AddDays(7),
            //     SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            // };
            // var token = tokenHandler.CreateToken(tokenDescriptor);
            // var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            // return Ok(new
            // {
            //     Id = user.Id,
            //     Username = user.Username,
            //     FirstName = user.FirstName,
            //     LastName = user.LastName,
            //     // Token = tokenString
            // });
            return Ok();
        }

        //GET api/user
        //get all users
        //TODO: limit with skip & take, filter etc. like devextreme params
        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers() {
            return Ok(_qe.GetAllUsers());
        }

        //GET api/user/id
        //get a specific user
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(uint id)
        {
            var user = _qe.GetUser(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        //POST api/user
        //create/register a user
        [HttpPost]
        public ActionResult CreateUser(User user)
        {
            var newUser = _qe.CreateUser(user);

            if (newUser == null)
                return NotFound();
                
            return Ok(newUser);
        }

        // PUT api/user/id
        // update user data (excluding id)
        [HttpPut("{id}")]
        public ActionResult UpdateUser(uint id, [FromBody] Dictionary<string, JsonElement> updates)
        {
            var user = _qe.GetUser(id);

            if (user == null)
                return NotFound();

            _qe.UpdateUser(id, updates);
            return NoContent();
        }

        // DELETE api/user/id
        // delete a specific user
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(uint id)
        {
            var user = _qe.GetUser(id);

            if (user == null)
                return NotFound();

            _qe.DeleteUser(id);
            return NoContent();
        }
    }
}