using System.Collections.Generic;
using Smartfinance_server.Models;

namespace Smartfinance_server.Data
{
	public interface IRepository
    {
        IEnumerable<Asset> GetAllAssets();
        Asset GetAssetById(int id);
    }
}