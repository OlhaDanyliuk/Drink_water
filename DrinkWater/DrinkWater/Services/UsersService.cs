﻿using System.Linq;

namespace DrinkWater.Services
{
    public class UsersService
    {
        private readonly dfkg9ojh16b4rdContext _db = null;

        private static UsersService instance = null;

        private UsersService()
        {
            _db = new dfkg9ojh16b4rdContext();
        }

        public static UsersService GetService
        {
            get
            {
                if (instance == null)
                {
                    instance = new UsersService();
                }

                return instance;
            }
        }

        public bool UsernameExists(string username)
        {
            var resultName = (from data in _db.Users
                              where data.Username == username
                              select data.Username).ToList();

            return resultName.Count > 0;
        }

        public bool EmailExists(string email)
        {
            var resultEmail = (from data in _db.Users
                               where data.Email == email
                               select data.Email).ToList();

            return resultEmail.Count > 0;
        }

        public void RegisterUser(Users user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public string GetUserSalt(string username)
        {
            var salt = (from data in _db.Users
                        where data.Username != null && data.Username == username
                        select data.Salt).FirstOrDefault();

            return salt;
        }

        public int GetUserId(string username, string password,  string salt)
        {
            string hashedPassword = EncryptionService.ComputeSaltedHash(password, int.Parse(salt));

            var userId = (from data in _db.Users
                          where data.Username != null && data.Username == username && data.Password == hashedPassword
                          select data.UserId).FirstOrDefault();

            return userId;
        }
    }
}
