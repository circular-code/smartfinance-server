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
        [HttpGet]
        public ActionResult <IEnumerable<Asset>> GetAllAssets() {
            var assets = _qe.GetAllAssets();

            return Ok(assets);
        }

        //GET api/assets/id
        [HttpGet("{id}")]
        public ActionResult <Asset> GetAssetById(uint id)
        {
            var asset = _qe.GetAssetById(id);
            return Ok(asset);
        }
    }
}