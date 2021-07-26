using System;
using System.Text;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Smartfinance_server.Models;
using Smartfinance_server.Helpers;
using System.Text.Json;

namespace Smartfinance_server.Data
{
    public class QueryEngine
    {

        //https://dev.mysql.com/doc/connector-net/en/connector-net-tutorials-sql-command.html

        // ExecuteReader to query the database. Results are usually returned in a MySqlDataReader object, created by ExecuteReader.

        // ExecuteNonQuery to insert, update, and delete data.

        // implement sql helper? https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcommand?view=dotnet-plat-ext-5.0

        // https://www.youtube.com/watch?v=fmvcAzHpsk8 Les Jackson MVC Rest API Course

        #region Asset
            
        public IEnumerable<Asset> GetAllAssets(uint userId)
        {
            List<Asset> list = new List<Asset>();

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM asset WHERE UserId = " + userId.ToString(), conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Asset()
                        {
                            UserId = Convert.ToUInt32(reader["UserId"]),
                            Id = Convert.ToUInt32(reader["Id"]),
                            CreationDate = reader["CreationDate"].ToString(),
                            ContractDate = reader["ContractDate"].ToString(),
                            CurrentValue = Convert.ToDecimal(reader["CurrentValue"]),
                            Currency = reader["Currency"].ToString(),
                            PrimaryTransactionId = Convert.ToUInt32(reader["PrimaryTransactionId"]),
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

        public Asset GetAsset(uint id, uint userId)
        {

            Asset? asset = null;

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM asset WHERE Id = " + id + " AND UserId = " + userId.ToString(), conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        asset = new Asset(){
                            UserId = Convert.ToUInt32(reader["UserId"]),
                            Id = Convert.ToUInt32(reader["Id"]),
                            CreationDate = reader["CreationDate"].ToString(),
                            ContractDate = reader["ContractDate"].ToString(),
                            CurrentValue = Convert.ToDecimal(reader["CurrentValue"]),
                            Currency = reader["Currency"].ToString(),
                            PrimaryTransactionId = Convert.ToUInt32(reader["PrimaryTransactionId"]),
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

        public Asset CreateAsset(Asset asset, uint userId)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO asset (UserId,CreationDate,ContractDate,CurrentValue,Currency,PrimaryTransactionId,Description,Type,CurrentQuantity,LiabilityIds,TransactionIds) VALUES (@UserId,@CreationDate,@ContractDate,@CurrentValue,@Currency,@PrimaryTransactionId,@Description,@Type,@CurrentQuantity,@LiabilityIds,@TransactionIds)", conn);
                cmd.Parameters.Add("@UserId",               MySqlDbType.UInt32).Value = userId;
                cmd.Parameters.Add("@CreationDate",         MySqlDbType.VarChar).Value = asset.CreationDate;
                cmd.Parameters.Add("@ContractDate",         MySqlDbType.VarChar).Value = asset.ContractDate;
                cmd.Parameters.Add("@CurrentValue",         MySqlDbType.Decimal).Value = asset.CurrentValue;
                cmd.Parameters.Add("@Currency",             MySqlDbType.VarChar).Value = asset.Currency;
                cmd.Parameters.Add("@PrimaryTransactionId", MySqlDbType.UInt32 ).Value = asset.PrimaryTransactionId;
                cmd.Parameters.Add("@Description",          MySqlDbType.VarChar).Value = asset.Description;
                cmd.Parameters.Add("@Type",                 MySqlDbType.VarChar).Value = asset.Type;
                cmd.Parameters.Add("@CurrentQuantity",      MySqlDbType.Decimal).Value = asset.CurrentQuantity;
                cmd.Parameters.Add("@LiabilityIds",         MySqlDbType.VarChar).Value = asset.LiabilityIds;
                cmd.Parameters.Add("@TransactionIds",       MySqlDbType.VarChar).Value = asset.TransactionIds;

                cmd.ExecuteNonQuery();

                // TODO: this might be an issue when db calls are made async
                MySqlCommand cmd2 = new MySqlCommand("select * from asset where id = LAST_INSERT_ID()", conn);

                using (var reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        asset = new Asset(){
                            UserId = Convert.ToUInt32(reader["UserId"]),
                            Id = Convert.ToUInt32(reader["Id"]),
                            CreationDate = reader["CreationDate"].ToString(),
                            ContractDate = reader["ContractDate"].ToString(),
                            CurrentValue = Convert.ToDecimal(reader["CurrentValue"]),
                            Currency = reader["Currency"].ToString(),
                            PrimaryTransactionId = Convert.ToUInt32(reader["PrimaryTransactionId"]),
                            Description = reader["Description"].ToString(),
                            Type = reader["Type"].ToString(),
                            CurrentQuantity = Convert.ToDecimal(reader["CurrentQuantity"]),
                            LiabilityIds = reader["LiabilityIds"].ToString(),
                            TransactionIds = reader["TransactionIds"].ToString()
                        };
                    }
                }
                conn.Close();
            }
            return asset;
        }

        public object UpdateAsset(uint id, uint userId, Dictionary<string, JsonElement> updates)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                
                StringBuilder sb = new StringBuilder("");
                MySqlCommand cmd = new MySqlCommand("", conn);

                foreach(KeyValuePair<string, JsonElement> kvp in updates)
                {
                    switch (kvp.Key) {
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

                        case "userId":
                        case "primaryTransactionId":
                            sb.Insert(0, kvp.Key + "=@" + kvp.Key + ",");
                            cmd.Parameters.Add("@" + kvp.Key, MySqlDbType.UInt32).Value = kvp.Value.GetUInt32();
                            break;
                    }
                }

                cmd.CommandText = "UPDATE asset SET " + sb.ToString().TrimEnd(',') + " WHERE id = " + id + " AND UserId = " + userId.ToString();

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return id;
        }

        public void DeleteAsset(uint id, uint userId)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM asset WHERE id = " + id + " AND UserId = " + userId.ToString(), conn);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        #endregion

        #region Transaction
            
        public IEnumerable<Transaction> GetAllTransactions(uint userId)
        {
            List<Transaction> list = new List<Transaction>();

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM transaction WHERE UserId = " + userId.ToString(), conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Transaction()
                        {
                            UserId = Convert.ToUInt32(reader["UserId"]),
                            Id = Convert.ToUInt32(reader["Id"]),
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

        public Transaction GetTransaction(uint id, uint userId)
        {
            Transaction transaction = new Transaction();

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM transaction WHERE Id = " + id + " AND UserId = " + userId.ToString(), conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transaction = new Transaction(){
                            UserId = Convert.ToUInt32(reader["UserId"]),
                            Id = Convert.ToUInt32(reader["Id"]),
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

        public Transaction CreateTransaction(List<Transaction> transactions, uint userId)
        {
            Transaction transaction = new Transaction();
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();

                //(@UserId, @BookingDate, @ValueDate, @Amount, @Currency, @Description, @Type, @Saldo, @Counterparty)

                MySqlCommand cmd = new MySqlCommand("", conn);
                string cmdString = "INSERT INTO transaction (UserId,BookingDate,ValueDate,Amount,Currency,Description,Type,Saldo,Counterparty) VALUES ";
                int counter = 0;

                foreach (Transaction t in transactions)
                {
                    cmdString += "(@UserId" + counter + ", @BookingDate" + counter + ", @ValueDate" + counter + ", @Amount" + counter + ", @Currency" + counter + ", @Description" + counter + ", @Type" + counter + ", @Saldo" + counter + ", @Counterparty" + counter + "),";
                    cmd.Parameters.Add("@UserId" + counter, MySqlDbType.UInt32).Value = userId;
                    cmd.Parameters.Add("@BookingDate" + counter, MySqlDbType.VarChar).Value = t.BookingDate;
                    cmd.Parameters.Add("@ValueDate" + counter, MySqlDbType.VarChar).Value = t.ValueDate;
                    cmd.Parameters.Add("@Amount" + counter, MySqlDbType.Decimal).Value = t.Amount;
                    cmd.Parameters.Add("@Currency" + counter, MySqlDbType.VarChar).Value = t.Currency;
                    cmd.Parameters.Add("@Description" + counter, MySqlDbType.VarChar).Value = t.Description;
                    cmd.Parameters.Add("@Type" + counter, MySqlDbType.VarChar).Value = t.Type;
                    cmd.Parameters.Add("@Saldo" + counter, MySqlDbType.Decimal).Value = t.Saldo;
                    cmd.Parameters.Add("@Counterparty" + counter, MySqlDbType.VarChar).Value = t.Counterparty;
                    counter++;
                }

                cmd.CommandText = cmdString.Substring(0, cmdString.Length - 1);
                cmd.ExecuteNonQuery();

                if (transactions.Count == 1)
                {
                    // TODO: this might be an issue when db calls are made async
                    MySqlCommand cmd2 = new MySqlCommand("select * from transaction where id = LAST_INSERT_ID()", conn);

                    using (var reader = cmd2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transaction = new Transaction(){
                                UserId = Convert.ToUInt32(reader["UserId"]),
                                Id = Convert.ToUInt32(reader["Id"]),
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

                conn.Close();
            }
            return transaction;
        }

        public object UpdateTransaction(uint id, uint userId, Dictionary<string, JsonElement> updates)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                
                StringBuilder sb = new StringBuilder("");
                MySqlCommand cmd = new MySqlCommand("", conn);

                foreach(KeyValuePair<string, JsonElement> kvp in updates) 
                {
                    switch (kvp.Key) {

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

                        case "userId":
                            sb.Insert(0, kvp.Key + "=@" + kvp.Key + ",");
                            cmd.Parameters.Add("@" + kvp.Key, MySqlDbType.UInt32).Value = kvp.Value.GetInt32();
                            break;
                    }
                }

                cmd.CommandText = "UPDATE transaction SET " + sb.ToString().TrimEnd(',') + " WHERE Id = " + id + " AND UserId = " + userId.ToString();

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return id;
        }

        public void DeleteTransaction(uint id, uint userId)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM transaction WHERE Id = " + id + " AND UserId = " + userId.ToString(), conn);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        #endregion
    
        #region User
            
        public IEnumerable<User> GetAllUsers()
        {
            List<User> list = new List<User>();

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from user", conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new User()
                        {
                            Id = Convert.ToUInt32(reader["Id"]),
                            Email = reader["Email"].ToString(),
                            Firstname = reader["Firstname"].ToString(),
                            Lastname = reader["Lastname"].ToString(),
                        });
                    }
                }
            }
            return list;
        }

        public User GetUserById(uint id)
        {

            User user = new User();

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from user where id = " + id, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User(){
                            Id = Convert.ToUInt32(reader["Id"]),
                            Email = reader["Email"].ToString(),
                            Firstname = reader["Firstname"].ToString(),
                            Lastname = reader["Lastname"].ToString(),
                        };
                    }
                }
            }
            return user;
        }

        public User GetUserByEmail(string email)
        {
            User user = new User();

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user WHERE Email = \"" + email + "\"", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User()
                        {
                            Id = Convert.ToUInt32(reader["Id"]),
                            Email = reader["Email"].ToString(),
                            Firstname = reader["Firstname"].ToString(),
                            Lastname = reader["Lastname"].ToString(),
                        };
                    }
                }
            }
            return user;
        }

        public User GetUserWithHashByEmail(string email)
        {
            User user = new User();

            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user WHERE Email = \"" + email + "\"", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User()
                        {
                            Id = Convert.ToUInt32(reader["Id"]),
                            Email = reader["Email"].ToString(),
                            Firstname = reader["Firstname"].ToString(),
                            Lastname = reader["Lastname"].ToString(),
                            PasswordHash = (byte[])reader["PasswordHash"],
                            PasswordSalt = (byte[])reader["PasswordSalt"],
                        };
                    }
                }
            }
            return user;
        }

        public User CreateUser(User user)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO user (Email,Firstname,Lastname,PasswordHash,PasswordSalt) VALUES (@Email,@Firstname,@Lastname,@PasswordHash,@PasswordSalt)", conn);
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = user.Email;
                cmd.Parameters.Add("@Firstname", MySqlDbType.VarChar).Value = user.Firstname;
                cmd.Parameters.Add("@Lastname", MySqlDbType.VarChar).Value = user.Lastname;
                cmd.Parameters.Add("@PasswordHash", MySqlDbType.Blob).Value = user.PasswordHash;
                cmd.Parameters.Add("@PasswordSalt", MySqlDbType.Blob).Value = user.PasswordSalt;

                cmd.ExecuteNonQuery();

                // TODO: this might be an issue when db calls are made async
                MySqlCommand readCmd = new MySqlCommand("select * from user where id = LAST_INSERT_ID()", conn);

                using (var reader = readCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User(){
                            Id = Convert.ToUInt32(reader["Id"]),
                            Email = reader["Email"].ToString(),
                            Firstname = reader["Firstname"].ToString(),
                            Lastname = reader["Lastname"].ToString(),
                        };
                    }
                }
                conn.Close();
            }
            return user;
        }

        public object UpdateUser(uint id, Dictionary<string, JsonElement> updates)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();
                
                StringBuilder sb = new StringBuilder("");
                MySqlCommand cmd = new MySqlCommand("", conn);

                foreach(KeyValuePair<string, JsonElement> kvp in updates)
                {
                    switch (kvp.Key) {
                        case "email":
                        case "firstname":
                        case "lastname":
                            sb.Insert(0, kvp.Key + "=@" + kvp.Key + ",");
                            cmd.Parameters.Add("@" + kvp.Key, MySqlDbType.VarChar).Value = kvp.Value.GetString();
                            break;

                        case "password":
                            UserHelper.CreatePasswordHash(kvp.Value.GetString(), out byte[] hash, out byte[] salt);

                            sb.Insert(0, "PasswordHash=@PasswordHash,PasswordSalt=@PasswordSalt,");
                            cmd.Parameters.Add("@PasswordHash", MySqlDbType.Blob).Value = hash;
                            cmd.Parameters.Add("@PasswordSalt", MySqlDbType.Blob).Value = salt;
                            break;
                    }
                }

                cmd.CommandText = "UPDATE user SET " + sb.ToString().TrimEnd(',') + " WHERE id = " + id;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return id;
        }

        public void DeleteUser(uint id)
        {
            using (MySqlConnection conn = DbContext.GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM user WHERE id = " + id, conn);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        #endregion
    }
}