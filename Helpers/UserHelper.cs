using System;
using System.Collections.Generic;
using System.Linq;
using Smartfinance_server.Data;
using Smartfinance_server.Models;
// using WebApi.Entities;
// using WebApi.Helpers;

namespace Smartfinance_server.Helpers {

    public class UserHelper
    {

        private readonly QueryEngine _qe;

        public UserHelper(QueryEngine qe)
        {
            _qe = qe;
        }

        // public User Authenticate(string username, string password)
        // {
        //     // if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        //     //     return null;

        //     // var user = _qe.Users.SingleOrDefault(x => x.Username == username);

        //     // // check if username exists
        //     // if (user == null)
        //     //     return null;

        //     // // check if password is correct
        //     // if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        //     //     return null;

        //     // authentication successful
        //     return User user;
        // }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}

