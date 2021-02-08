using System.Collections.Generic;
using Smartfinance_server.Models;

namespace Smartfinance_server.Data
{
    public class MockRepository : IRepository
    {
        public IEnumerable<Asset> GetAllAssets()
        {
            return new List<Asset>
            {
                new Asset{Name="Auto", Value=30000, Debt=18000},
                new Asset{Name="Haus", Value=120000, Debt=80000},
                new Asset{Name="Studium", Value=34000, Debt=8000}
            };
        }

        public Asset GetAssetById(uint id)
        {
            return new Asset{Name="test", Value=12, Debt= 8};
        }
    }
}