using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Smartfinance_server.Data;
using Smartfinance_server.Models;
using System.Text.Json;

// https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing
// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio
// https://docs.microsoft.com/en-us/dotnet/api/system.web.http.apicontroller?view=aspnetcore-2.2

namespace Smartfinance_server.Controllers

{
    [Route("api/assets")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly QueryEngine _qe;

        public AssetsController(QueryEngine qe)
        {
            _qe = qe;
        }

        //GET api/assets
        //get all assets
        //TODO: limit with skip & take, filter etc. like devextreme params
        [HttpGet]
        public ActionResult<IEnumerable<Asset>> GetAllAssets() {
            return Ok(_qe.GetAllAssets());
        }

        //GET api/assets/id
        //get a specific assets
        [HttpGet("{id}")]
        public ActionResult<Asset> GetAsset(uint id)
        {
            var asset = _qe.GetAsset(id);

            if (asset == null)
                return NotFound();

            return Ok(asset);
        }

        //POST api/assets
        //create a asset in full
        [HttpPost]
        public ActionResult CreateAsset(Asset asset)
        {
            var newAsset = _qe.CreateAsset(asset);

            if (newAsset == null)
                return NotFound();
                
            return NoContent();
        }

        // PUT api/assets/id
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

        // DELETE api/assets/id
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