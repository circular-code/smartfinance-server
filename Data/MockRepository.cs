using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Smartfinance_server.Models;

namespace Smartfinance_server.Data
{
    public class MockRepository : IRepository
    {
        public IEnumerable<Asset> GetAllAssets()
        {
            List<Asset> list = new List<Asset>();

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Assets where id < 10", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Asset()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Value = Convert.ToInt32(reader["Value"]),
                            Debt = Convert.ToInt32(reader["Debt"])
                        });
                    }
                }
            }
            return list;
        }

        public Asset GetAssetById(uint id)
        {
            return new Asset{Name="test", Value=12, Debt= 8};
        }
    }
}