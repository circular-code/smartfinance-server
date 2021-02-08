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

        private readonly IRepository _repo;

        public AssetsController(IRepository repo)
        {
            _repo = repo;
        }

        //GET api/assets
        [HttpGet]
        public ActionResult <IEnumerable<Asset>> GetAllAssets() {
            var assets = _repo.GetAllAssets();

            return Ok(assets);
        }

        //GET api/assets/id
        [HttpGet("{id}")]
        public ActionResult <Asset> GetAssetById(uint id)
        {
            var asset = _repo.GetAssetById(id);
            return Ok(asset);
        }
    }
}