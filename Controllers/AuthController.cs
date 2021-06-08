using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Smartfinance_server.Data;
using Smartfinance_server.Models;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Smartfinance_server.Controllers

{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // private readonly QueryEngine _qe;

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authenticate()
        {
            // 1. user schickt username + pw
            // 2. wir überprüfen ob user vorhanden
            // 3. wir hashen password und überprüfen pw hash
            // 4. ClaimsPrincipal erstellen anhand von userdaten
            
            // 5. neue Request vom user mit cookie authorisiert und liefert info über user identifikation die in endpunkt verwendet kann, aus dem cookie muss 

            List<Claim> ids = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Steph"),
                new Claim(ClaimTypes.Email, "steph@steph.com"),
                new Claim(ClaimTypes.DateOfBirth, "11/11/1111"),
            };

            ClaimsIdentity identity = new ClaimsIdentity(ids, "BasicDataIdentity");

            var userPrincipal = new ClaimsPrincipal(new[] { identity });

            await HttpContext.SignInAsync(userPrincipal);

            return Ok();
        }
    }
}
