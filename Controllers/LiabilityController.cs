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
using System.Security.Claims;
using System;
using Smartfinance_server.Helpers;

// https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing
// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio
// https://docs.microsoft.com/en-us/dotnet/api/system.web.http.apicontroller?view=aspnetcore-2.2


//TODO: logging

namespace Smartfinance_server.Controllers

{
    [Route("api/liability")]
    [ApiController]
    public class LiabilityController : ControllerBase
    {
        private readonly QueryEngine _qe;

        public LiabilityController(QueryEngine qe)
        {
            _qe = qe;
        }

        //GET api/liability
        //get all liabilities
        //TODO: limit with skip & take, filter etc. like devextreme params
        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<Liability>> GetAllLiabilities() {
            if (HttpContext.Request.Cookies.TryGetValue("Identity.Cookie", out string cookieValue))
            {
                //System.Diagnostics.Debug.WriteLine(cookieValue);

                //throw new Exception();

                if (!UserHelper.TryGetUserIdFromCookie(HttpContext.User, out uint userId))
                    return Problem("Could not find userId in cookie");

                return Ok(_qe.GetAllLiabilities(userId));
            }
            return BadRequest();
        }

        //GET api/liability/id
        //get a specific liability
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Liability> GetLiability(uint id)
        {
            if (!UserHelper.TryGetUserIdFromCookie(HttpContext.User, out uint userId))
                return Problem("Could not find userId in cookie");

            var liability = _qe.GetLiability(id, userId);

            if (liability == null)
                return NotFound();

            return Ok(liability);
        }

        //POST api/liability
        //create a liability in full
        [Authorize]
        [HttpPost]
        public ActionResult CreateLiability(Liability liability)
        {
            if (!UserHelper.TryGetUserIdFromCookie(HttpContext.User, out uint userId))
                return Problem("Could not find userId in cookie");

            var newLiability = _qe.CreateLiability(liability, userId);

            if (newLiability == null)
                return NotFound();
                
            return Ok(newLiability);
        }

        // PUT api/liability/id
        // update liability data (excluding id)
        // we are missusing PUT to Update existing Liabilities instead of PATCH since PATCH requres additional dependencies (jsonpatch) and a different endpoint-style; see https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult UpdateLiability(uint id, [FromBody] Dictionary<string, JsonElement> updates)
        {
            if (!UserHelper.TryGetUserIdFromCookie(HttpContext.User, out uint userId))
                return Problem("Could not find userId in cookie");

            var liability = _qe.GetLiability(id, userId);

            if (liability == null)
                return NotFound();

            _qe.UpdateLiability(id, userId, updates);
            return NoContent();
        }

        // DELETE api/liability/id
        // delete a specific liability
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult DeleteLiability(uint id)
        {
            if (!UserHelper.TryGetUserIdFromCookie(HttpContext.User, out uint userId))
                return Problem("Could not find userId in cookie");

            var liability = _qe.GetLiability(id, userId);

            if (liability == null)
                return NotFound();

            _qe.DeleteLiability(id, userId);
            return NoContent();
        }
    }
}