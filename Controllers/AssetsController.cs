using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Smartfinance_server.Data;
using Smartfinance_server.Models;

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
        public ActionResult <IEnumerable<Asset>> GetAllAssets() {
            var assets = _qe.GetAllAssets();

            return Ok(assets);
        }

        //GET api/assets/id
        //get a specific assets
        [HttpGet("{id}")]
        public ActionResult <Asset> GetAssetById(uint id)
        {
            var asset = _qe.GetAssetById(id);
            return Ok(asset);
        }

        //PUT api/assets/id
        //update/replace the whole asset
        [HttpPut("{id}")]
        public ActionResult <Asset> GetAssetById(uint id)
        {
            var asset = _qe.GetAssetById(id);
            return Ok(asset);
        }

        //POST api/assets
        //create a asset in full
        [HttpPost]
        public ActionResult <Asset> Create(Asset asset)
        {
            var asset = _qe.CreateAsset(asset);
            return CreatedAtAction(asset);
        }

        //PATCH api/assets/id
        //update part of an asset
        [HttpPatch("{id}")]
        public ActionResult <Asset> Create(Asset asset)
        {
            var asset = _qe.Create(asset);
            return CreatedAtAction(asset);
        }

        //DELETE api/assets/id
        //delete a specific asset
        [HttpDelete("{id}")]
        public ActionResult <Asset> Create(Asset asset)
        {
            var asset = _qe.CreateAsset(asset);
            return CreatedAtAction(asset);
        }

        //HEAD api/assets
        //efficiently lookup whether large assets have been updated in conjunction with the ETag-header.
        [HttpHead("{id}")]
        public ActionResult <Asset> GetAssetById(uint id)
        {
            var asset = _qe.GetAssetById(id);
            return Ok(asset);
        }
    }
}