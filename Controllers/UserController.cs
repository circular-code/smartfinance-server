using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Smartfinance_server.Data;
using Smartfinance_server.Models;
using Smartfinance_server.Helpers;
using System.Text.Json;
using System.Security.Claims;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;

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
        public async Task<IActionResult> Login([FromBody] Dictionary<string, JsonElement> userData)
        {
            string Email = "";
            string Password = "";

            foreach (KeyValuePair<string, JsonElement> kvp in userData)
                if (kvp.Key == "email")
                    Email = kvp.Value.GetString();
                else if (kvp.Key == "password")
                    Password = kvp.Value.GetString();

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                return BadRequest();

            User user = _qe.GetUserByEmail(Email);

            // check if user exists
            if (user == null)
                return NotFound();

            //TODO: implement getUser with hash data for login

            // check if password is correct
            if (!UserHelper.VerifyPasswordHash(Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest();

            //authentication successful

            // 4. ClaimsPrincipal erstellen anhand von userdaten

            List<Claim> ids = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, Email),
            };

            ClaimsIdentity identity = new ClaimsIdentity(ids, "BasicDataIdentity");

            var userPrincipal = new ClaimsPrincipal(new[] { identity });

            await HttpContext.SignInAsync(userPrincipal);

            return Ok();

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
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(uint id)
        {
            var user = _qe.GetUserById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        //POST api/user
        //register a user
        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult Register([FromBody] Dictionary<string, JsonElement> userData)
        {
            string Email = "";
            string Password = "";
            string Firstname = "";
            string Lastname = "";

            foreach (KeyValuePair<string, JsonElement> kvp in userData)
                if (kvp.Key == "email")
                    Email = kvp.Value.GetString();
                else if (kvp.Key == "password")
                    Password = kvp.Value.GetString();
                else if (kvp.Key == "firstname")
                    Firstname = kvp.Value.GetString();
                else if (kvp.Key == "lastname")
                    Lastname = kvp.Value.GetString();

            // validation
            if (string.IsNullOrWhiteSpace(Password))
                throw new ArgumentException("Password is required");

            User user = _qe.GetUserByEmail(Email);

            if (user.Email != null)
                throw new ArgumentException("Email \"" + Email + "\" is already taken");
 
            user.Email = Email;
            user.Firstname = Firstname;
            user.Lastname = Lastname;

            byte[] passwordHash, passwordSalt;
            UserHelper.CreatePasswordHash(Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var newUser = _qe.CreateUser(user);

            if (newUser == null)
                return Problem();
                
            return Ok(newUser);
        }

        // PUT api/user/id
        // update user data (excluding id)
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult UpdateUser(uint id, [FromBody] Dictionary<string, JsonElement> updates)
        {
            var user = _qe.GetUserById(id);

            if (user == null)
                return NotFound();

            _qe.UpdateUser(id, updates);
            return NoContent();
        }

        // DELETE api/user/id
        // delete a specific user
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(uint id)
        {
            //TODO: only allow to delete own user, then redirect to start, maybe dont really delete user, instead set user inactive? Datenschutz
            var user = _qe.GetUserById(id);

            if (user == null)
                return NotFound();

            _qe.DeleteUser(id);
            return NoContent();
        }
    }
}