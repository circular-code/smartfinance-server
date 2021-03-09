using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Smartfinance_server.Data;
using Smartfinance_server.Models;

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
        public ActionResult<Asset> CreateAsset(Asset asset)
        {
            var newAsset = _qe.CreateAsset(asset);

            if (newAsset == null)
                return NotFound();
                
            return NoContent();
        }

        // since PATCH requres additional dependencies (jsonpatch), we are not going to use it
        //PATCH api/assets/id
        //update part of an asset
        // [HttpPatch("{id}")]
        // public ActionResult<Asset> UpdateAsset(uint id)
        // {
        //     return Ok(_qe.UpdateAsset(asset));
        // }

        // we are missusing PUT to Update existing Assets instead of patch
        //PUT api/assets/id
        //replace asset if existing or create the asset if not existing
        // [HttpPut("{id}")]
        // public ActionResult <Asset> ReplaceAsset(uint id)
        // {
        //     return Ok(_qe.ReplaceAsset(id));
        // }

        // DELETE api/assets/id
        // delete a specific asset
        [HttpDelete("{id}")]
        public ActionResult<Asset> DeleteAsset(uint id)
        {
            var asset = _qe.GetAsset(id);

            if (asset == null)
                return NotFound();

            _qe.DeleteAsset(id);
            return NoContent();
        }

        //HEAD api/assets
        //efficiently lookup whether large assets have been updated in conjunction with the ETag-header.
        // [HttpHead("{id}")]
        // public ActionResult <Asset> GetAssetById(uint id)
        // {
        //     var asset = _qe.GetAssetById(id);
        //     return Ok(asset);
        // }
    }
}