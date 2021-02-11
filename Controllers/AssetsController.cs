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
        //update a specific asset in full
        //TODO: probably not necessary
        [HttpPut("{id}")]
        public ActionResult <Asset> GetAssetById(uint id)
        {
            var asset = _qe.GetAssetById(id);
            return Ok(asset);
        }

        //PUT api/assets/id
        //update a specific asset in full
        //TODO: probably not necessary
        [HttpPut("{id}")]
        public ActionResult <Asset> GetAssetById(uint id)
        {
            var asset = _qe.GetAssetById(id);
            return Ok(asset);
        }

        //HEAD api/assets
        //efficiently lookup whether large assets have been updated in conjunction with the ETag-header.
        //TODO: probably not necessary
        [HttpHead("{id}")]
        public ActionResult <Asset> GetAssetById(uint id)
        {
            var asset = _qe.GetAssetById(id);
            return Ok(asset);
        }
    }
}