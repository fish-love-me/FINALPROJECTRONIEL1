using project_machshevon.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectRoniel.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private SharedViewModel _sharedViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserViewModel(SharedViewModel sharedViewModel)
        {
            _sharedViewModel = sharedViewModel;
            LoadUsersFromDatabase(); // פעולה הקורא לטבלת המשתמשים
        }
        public void RefreshUsers()
        {
            LoadUsersFromDatabase(); //פעולה הקורא לטבלת המשתמשים
        }


        private void LoadUsersFromDatabase()
        {
            // Update the path to your actual database file
            string connectionString = @"Data Source=C:\Users\ronie\source\repos\mvvm\project_machshevon\table1.db;Version=3;";
            var usersList = new List<User>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Users";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usersList.Add(new User
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                FirstName = Convert.ToString(reader["FirstName"]),
                                LastName = Convert.ToString(reader["LastName"]),
                                City = Convert.ToString(reader["City"]),
                                State = Convert.ToString(reader["State"]),
                                Country = Convert.ToString(reader["Country"]),
                                Email = Convert.ToString(reader["Email"]),
                                Password = Convert.ToString(reader["Password"]),
                            });
                        }
                    }
                }
            }

            _sharedViewModel.UsersList = usersList; // Update the SharedViewModel's UsersList
            OnPropertyChanged(nameof(Users)); // Notify that the Users list has been updated
        }

        public List<User> Users
        {
            get { return _sharedViewModel.UsersList; }
        }
    }
}
