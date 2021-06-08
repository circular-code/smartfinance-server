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

// https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing
// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio
// https://docs.microsoft.com/en-us/dotnet/api/system.web.http.apicontroller?view=aspnetcore-2.2

namespace Smartfinance_server.Controllers

{
    [Route("api/asset")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly QueryEngine _qe;

        public AssetController(QueryEngine qe)
        {
            _qe = qe;
        }

        //GET api/asset
        //get all assets
        //TODO: limit with skip & take, filter etc. like devextreme params
        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<Asset>> GetAllAssets() {
            if (HttpContext.Request.Cookies.TryGetValue("Identity.Cookie", out string cookieValue))
            {
                System.Diagnostics.Debug.WriteLine(cookieValue);
                //new Claim()
                return Ok(_qe.GetAllAssets());
            }
            return BadRequest();
        }

        //GET api/asset/id
        //get a specific asset
        [HttpGet("{id}")]
        public ActionResult<Asset> GetAsset(uint id)
        {
            var asset = _qe.GetAsset(id);

            if (asset == null)
                return NotFound();

            return Ok(asset);
        }

        //POST api/asset
        //create a asset in full
        [HttpPost]
        public ActionResult CreateAsset(Asset asset)
        {
            var newAsset = _qe.CreateAsset(asset);

            if (newAsset == null)
                return NotFound();
                
            return Ok(newAsset);
        }

        // PUT api/asset/id
        // update asset data (excluding id)
        // we are missusing PUT to Update existing Assets instead of PATCH since PATCH requres additional dependencies (jsonpatch) and a different endpoint-style; see https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0
        [HttpPut("{id}")]
        public ActionResult UpdateAsset(uint id, [FromBody] Dictionary<string, JsonElement> updates)
        {
            var asset = _qe.GetAsset(id);

            if (asset == null)
                return NotFound();

            _qe.UpdateAsset(id, updates);
            return NoContent();
        }

        // DELETE api/asset/id
        // delete a specific asset
        [HttpDelete("{id}")]
        public ActionResult DeleteAsset(uint id)
        {
            var asset = _qe.GetAsset(id);

            if (asset == null)
                return NotFound();

            _qe.DeleteAsset(id);
            return NoContent();
        }
    }
}