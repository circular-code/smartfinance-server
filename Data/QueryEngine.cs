using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Smartfinance_server.Models;

namespace Smartfinance_server.Data
{
    public class QueryEngine
    {

        //https://dev.mysql.com/doc/connector-net/en/connector-net-tutorials-sql-command.html

        // ExecuteReader to query the database. Results are usually returned in a MySqlDataReader object, created by ExecuteReader.

        // ExecuteNonQuery to insert, update, and delete data.

        // ExecuteScalar to return a single value.

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
                            User = reader["User"].ToString(),
                            Id = Convert.ToInt32(reader["Id"]),
                            CreationDate = reader["CreationDate"].ToString(),
                            ContractDate = reader["ContractDate"].ToString(),
                            CurrentValue = reader["CurrentValue"],
                            Currency = reader["Currency"].ToString(),
                            PrimaryTransactionId = Convert.ToInt32(reader["PrimaryTransactionId"]),
                            Description = reader["Description"].ToString(),
                            Type = reader["Type"].ToString(),
                            CurrentQuantity = reader["CurrentQuantity"],
                            LiabilityIds = reader["LiabilityIds"].ToString(),
                            TransactionIds = reader["TransactionIds"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public Asset GetAssetById(uint id)
        {

            Asset asset = new Asset();

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Assets where id = " + id, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        asset = new Asset(){
                            User = reader["User"].ToString(),
                            Id = Convert.ToInt32(reader["Id"]),
                            CreationDate = reader["CreationDate"].ToString(),
                            ContractDate = reader["ContractDate"].ToString(),
                            CurrentValue = reader["CurrentValue"],
                            Currency = reader["Currency"].ToString(),
                            PrimaryTransactionId = Convert.ToInt32(reader["PrimaryTransactionId"]),
                            Description = reader["Description"].ToString(),
                            Type = reader["Type"].ToString(),
                            CurrentQuantity = reader["CurrentQuantity"],
                            LiabilityIds = reader["LiabilityIds"].ToString(),
                            TransactionIds = reader["TransactionIds"].ToString()
                        }
                    }
                }
            }
            return asset;
        }

        public Asset Create(Asset asset)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Assets where id = " + id, conn);

                cmd = new MySqlCommand("INSERT INTO mytable (Id,User,CreationDate,ContractDate,CurrentValue,Currency,PrimaryTransactionId,Description,Type,CurrentQuantity,LiabilityIds,TransactionIds) VALUES (@Id,@User,@CreationDate,@ContractDate,@CurrentValue,@Currency,@PrimaryTransactionId,@Description,@Type,@CurrentQuantity,@LiabilityIds,@TransactionIds)", conn);
                //TODO find out how to use parameters
                cmd.Parameters.Add("@Id", MySqlDbType.VarChar, 15, "Id" );
                cmd.Parameters.Add("@name", MySqlDbType.VarChar, 15, "name" );

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return asset;
        }
        
    }
}