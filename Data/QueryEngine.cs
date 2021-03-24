using System;
using System.Text;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Smartfinance_server.Models;
using System.Text.Json;

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
                MySqlCommand cmd = new MySqlCommand("select * from asset", conn);

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
                MySqlCommand cmd = new MySqlCommand("select * from asset where id = " + id, conn);

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

        public object UpdateAsset(uint id, Dictionary<string, JsonElement> updates)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                
                StringBuilder sb = new StringBuilder("");
                MySqlCommand cmd = new MySqlCommand("", conn);

                foreach(KeyValuePair<string, JsonElement> kvp in updates)
                {
                    switch (kvp.Key) {
                        case "user":
                        case "creationDate":
                        case "contractDate":
                        case "currency":
                        case "description":
                        case "type":
                        case "liabilityIds":
                        case "transactionIds":
                            sb.Insert(0, kvp.Key + "=@" + kvp.Key + ",");
                            cmd.Parameters.Add("@" + kvp.Key, MySqlDbType.VarChar).Value = kvp.Value.GetString();
                            break;

                        case "currentValue":
                        case "currentQuantity":
                            sb.Insert(0, kvp.Key + "=@" + kvp.Key + ",");
                            cmd.Parameters.Add("@" + kvp.Key, MySqlDbType.Decimal).Value = kvp.Value.GetDecimal();
                            break;

                        case "primaryTransactionId":
                            sb.Insert(0, kvp.Key + "=@" + kvp.Key + ",");
                            cmd.Parameters.Add("@" + kvp.Key, MySqlDbType.UInt16).Value = kvp.Value.GetInt16();
                            break;
                    }
                }

                cmd.CommandText = "UPDATE asset SET " + sb.ToString().TrimEnd(',') + " WHERE id = " + id;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return id;
        }

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

        #region Transaction
            
        public IEnumerable<Transaction> GetAllTransactions()
        {
            List<Transaction> list = new List<Transaction>();

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from transaction", conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Transaction()
                        {
                            User = reader["User"].ToString(),
                            Id = Convert.ToInt32(reader["Id"]),
                            BookingDate = reader["BookingDate"].ToString(),
                            ValueDate = reader["ValueDate"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            Currency = reader["Currency"].ToString(),
                            Description = reader["Description"].ToString(),
                            Type = reader["Type"].ToString(),
                            Saldo = Convert.ToDecimal(reader["Saldo"]),
                            Counterparty = reader["Counterparty"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public Transaction GetTransaction(uint id)
        {
            Transaction transaction = new Transaction();

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from transaction where id = " + id, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transaction = new Transaction(){
                            User = reader["User"].ToString(),
                            Id = Convert.ToInt32(reader["Id"]),
                            BookingDate = reader["BookingDate"].ToString(),
                            ValueDate = reader["ValueDate"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            Currency = reader["Currency"].ToString(),
                            Description = reader["Description"].ToString(),
                            Type = reader["Type"].ToString(),
                            Saldo = Convert.ToDecimal(reader["Saldo"]),
                            Counterparty = reader["Counterparty"].ToString()
                        };
                    }
                }
            }
            return transaction;
        }

        public Transaction CreateTransaction(Transaction transaction)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO transaction (User,BookingDate,ValueDate,Amount,Currency,Description,Type,Saldo,Counterparty) VALUES (@User,@BookingDate,@ValueDate,@Amount,@Currency,@Description,@Type,@Saldo,@Counterparty)", conn);
                cmd.Parameters.Add("@User",                 MySqlDbType.VarChar).Value = transaction.User;
                cmd.Parameters.Add("@BookingDate",          MySqlDbType.VarChar).Value = transaction.BookingDate;
                cmd.Parameters.Add("@ValueDate",            MySqlDbType.VarChar).Value = transaction.ValueDate;
                cmd.Parameters.Add("@Amount",               MySqlDbType.Decimal).Value = transaction.Amount;
                cmd.Parameters.Add("@Currency",             MySqlDbType.VarChar).Value = transaction.Currency;
                cmd.Parameters.Add("@Description",          MySqlDbType.VarChar).Value = transaction.Description;
                cmd.Parameters.Add("@Type",                 MySqlDbType.VarChar).Value = transaction.Type;
                cmd.Parameters.Add("@Saldo",                MySqlDbType.Decimal).Value = transaction.Saldo;
                cmd.Parameters.Add("@Counterparty",         MySqlDbType.VarChar).Value = transaction.Counterparty;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return transaction;
        }

        public object UpdateTransaction(uint id, Dictionary<string, JsonElement> updates)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                
                StringBuilder sb = new StringBuilder("");
                MySqlCommand cmd = new MySqlCommand("", conn);

                foreach(KeyValuePair<string, JsonElement> kvp in updates) 
                {
                    switch (kvp.Key) {

                        // case "user":
                        case "bookingDate":
                        case "valueDate":
                        case "currency":
                        case "description":
                        case "type":
                        case "counterparty":
                            sb.Insert(0, kvp.Key + "=@" + kvp.Key + ",");
                            cmd.Parameters.Add("@" + kvp.Key, MySqlDbType.VarChar).Value = kvp.Value.GetString();
                            break;

                        case "amount":
                        case "saldo":
                            sb.Insert(0, kvp.Key + "=@" + kvp.Key + ",");
                            cmd.Parameters.Add("@" + kvp.Key, MySqlDbType.Decimal).Value = kvp.Value.GetDecimal();
                            break;
                    }
                }

                cmd.CommandText = "UPDATE transaction SET " + sb.ToString().TrimEnd(',') + " WHERE id = " + id;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return id;
        }

        public void DeleteTransaction(uint id)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM transaction WHERE id = " + id, conn);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        #endregion
    }
}