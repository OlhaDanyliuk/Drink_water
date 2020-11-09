﻿using System.Windows;
using System.Linq;
using DrinkWater.Services;

namespace DrinkWater.LogReg
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        private string username;
        private string password;

        public Login()
        {
            InitializeComponent();
        }

        private void buttonCreateNewAccount_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }

        public bool isInDatabase(string item)
        {
            if (item == null)
            {
                return false;
            }

            return true;
        }

        private void buttonLogIn_Click(object sender, RoutedEventArgs e)
        {
            this.username = textBoxUsername.Text;
            this.password = textBoxPassword.Text;

            var salt = (from data in db.Users
                        where data.Username != null && data.Username == username//хешований пароль не витягувати перевіряти відразу через лінкю витягувати тільки солт.
                        select data.Salt).FirstOrDefault();//хешування паролю і сет ерор винести в окремий клас.

            if (salt != null)
            {
                labelUsername.Visibility = Visibility.Hidden;
                string hashedPassword = EncryptionService.ComputeSaltedHash(this.password, int.Parse(salt));

                var userId = (from data in db.Users
                              where data.Username != null && data.Username == username && data.Password == hashedPassword
                              select data.UserId).FirstOrDefault();

                if (userId > 0)
                {
                    labelPassword.Visibility = Visibility.Hidden;

                    SessionUser sessionUser = new SessionUser((long)userId, username);
                    MessageBox.Show("it works.");
                }
                else
                {
                    ValidationService.SetError(labelPassword, "Incorrect password");
                }
            }
            else
            {
                ValidationService.SetError(labelUsername, "No such username in database");
            }
        }
    }
}
