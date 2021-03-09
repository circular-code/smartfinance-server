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

        // ExecuteScalar to return a single value (first column of first row)

        // Improve SQL Security with Stored procedures and parameters

        // implement sql helper? https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcommand?view=dotnet-plat-ext-5.0

        // https://www.youtube.com/watch?v=fmvcAzHpsk8 Les Jackson MVC Rest API Course

        #region Asset
            
        public IEnumerable<Asset> GetAllAssets()
        {
            List<Asset> list = new List<Asset>();

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Asset", conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Asset()
                        {
                            User = reader["User"].ToString(),
                            Id = Convert.ToInt32(reader["Id"]),
                            CreationDate = reader["CreationDate"].ToString(),
                            ContractDate = reader["ContractDate"].ToString(),
                            CurrentValue = Convert.ToDecimal(reader["CurrentValue"]),
                            Currency = reader["Currency"].ToString(),
                            PrimaryTransactionId = Convert.ToInt32(reader["PrimaryTransactionId"]),
                            Description = reader["Description"].ToString(),
                            Type = reader["Type"].ToString(),
                            CurrentQuantity = Convert.ToDecimal(reader["CurrentQuantity"]),
                            LiabilityIds = reader["LiabilityIds"].ToString(),
                            TransactionIds = reader["TransactionIds"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public Asset GetAsset(uint id)
        {

            Asset asset = new Asset();

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Asset where id = " + id, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        asset = new Asset(){
                            User = reader["User"].ToString(),
                            Id = Convert.ToInt32(reader["Id"]),
                            CreationDate = reader["CreationDate"].ToString(),
                            ContractDate = reader["ContractDate"].ToString(),
                            CurrentValue = Convert.ToDecimal(reader["CurrentValue"]),
                            Currency = reader["Currency"].ToString(),
                            PrimaryTransactionId = Convert.ToInt32(reader["PrimaryTransactionId"]),
                            Description = reader["Description"].ToString(),
                            Type = reader["Type"].ToString(),
                            CurrentQuantity = Convert.ToDecimal(reader["CurrentQuantity"]),
                            LiabilityIds = reader["LiabilityIds"].ToString(),
                            TransactionIds = reader["TransactionIds"].ToString()
                        };
                    }
                }
            }
            return asset;
        }

        public Asset CreateAsset(Asset asset)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO asset (User,CreationDate,ContractDate,CurrentValue,Currency,PrimaryTransactionId,Description,Type,CurrentQuantity,LiabilityIds,TransactionIds) VALUES (@User,@CreationDate,@ContractDate,@CurrentValue,@Currency,@PrimaryTransactionId,@Description,@Type,@CurrentQuantity,@LiabilityIds,@TransactionIds)", conn);
                cmd.Parameters.Add("@User",                 MySqlDbType.VarChar).Value = asset.User;
                cmd.Parameters.Add("@CreationDate",         MySqlDbType.VarChar).Value = asset.CreationDate;
                cmd.Parameters.Add("@ContractDate",         MySqlDbType.VarChar).Value = asset.ContractDate;
                cmd.Parameters.Add("@CurrentValue",         MySqlDbType.Decimal).Value = asset.CurrentValue;
                cmd.Parameters.Add("@Currency",             MySqlDbType.VarChar).Value = asset.Currency;
                cmd.Parameters.Add("@PrimaryTransactionId", MySqlDbType.UInt16 ).Value = asset.PrimaryTransactionId;
                cmd.Parameters.Add("@Description",          MySqlDbType.VarChar).Value = asset.Description;
                cmd.Parameters.Add("@Type",                 MySqlDbType.VarChar).Value = asset.Type;
                cmd.Parameters.Add("@CurrentQuantity",      MySqlDbType.Decimal).Value = asset.CurrentQuantity;
                cmd.Parameters.Add("@LiabilityIds",         MySqlDbType.VarChar).Value = asset.LiabilityIds;
                cmd.Parameters.Add("@TransactionIds",       MySqlDbType.VarChar).Value = asset.TransactionIds;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return asset;
        }

        //TODO:
        // public Asset UpdateAsset(Asset asset)
        // {
        //     using (MySqlConnection conn = DbContext.GetConnection())
        //     {
        //         conn.Open();

        //         MySqlCommand cmd = new MySqlCommand("INSERT INTO asset (User,CreationDate,ContractDate,CurrentValue,Currency,PrimaryTransactionId,Description,Type,CurrentQuantity,LiabilityIds,TransactionIds) VALUES (@User,@CreationDate,@ContractDate,@CurrentValue,@Currency,@PrimaryTransactionId,@Description,@Type,@CurrentQuantity,@LiabilityIds,@TransactionIds)", conn);
        //         if (asset.User)
        //         cmd.Parameters.Add("@User",                 MySqlDbType.VarChar).Value = asset.User;
        //         cmd.Parameters.Add("@CreationDate",         MySqlDbType.VarChar).Value = asset.CreationDate;
        //         cmd.Parameters.Add("@ContractDate",         MySqlDbType.VarChar).Value = asset.ContractDate;
        //         cmd.Parameters.Add("@CurrentValue",         MySqlDbType.Decimal).Value = asset.CurrentValue;
        //         cmd.Parameters.Add("@Currency",             MySqlDbType.VarChar).Value = asset.Currency;
        //         cmd.Parameters.Add("@PrimaryTransactionId", MySqlDbType.UInt16 ).Value = asset.PrimaryTransactionId;
        //         cmd.Parameters.Add("@Description",          MySqlDbType.VarChar).Value = asset.Description;
        //         cmd.Parameters.Add("@Type",                 MySqlDbType.VarChar).Value = asset.Type;
        //         cmd.Parameters.Add("@CurrentQuantity",      MySqlDbType.Decimal).Value = asset.CurrentQuantity;
        //         cmd.Parameters.Add("@LiabilityIds",         MySqlDbType.VarChar).Value = asset.LiabilityIds;
        //         cmd.Parameters.Add("@TransactionIds",       MySqlDbType.VarChar).Value = asset.TransactionIds;

        //         cmd.ExecuteNonQuery();
        //         conn.Close();
        //     }
        //     return asset;
        // }

        public void DeleteAsset(uint id)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM asset WHERE id = " + id, conn);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        #endregion
    }
}