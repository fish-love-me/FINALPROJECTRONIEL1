using FinalProjectRoniel.ViewModel;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinalProjectRoniel.View
{
    public partial class Register : Page
    {
        private SharedViewModel _sharedViewModel;

        public Register(SharedViewModel sharedViewModel)
        {
            InitializeComponent();
            _sharedViewModel = sharedViewModel;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!AreAllValuesFilled())
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            string email = txtUser.Text;
            string firstName = txtName.Text;
            string lastName = txtFamily.Text;
            string password = txtPassword.Password; // Changed to PasswordBox
            string age = txtAge.Text;

            string connectionString = @"Data Source=C:\Users\ronie\source\repos\mvvm\project_machshevon\table1.db;Version=3;";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Users (Email, Password, FirstName, LastName, Age, IsAdmin) VALUES (@Email, @Password, @FirstName, @LastName, @Age, 0)";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Age", int.Parse(age));
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("User registered successfully!");
                NavigationService.Navigate(new Login(_sharedViewModel));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error registering user: {ex.Message}");
            }
        }

        private bool AreAllValuesFilled()
        {
            return !string.IsNullOrEmpty(txtUser.Text) &&
                   !string.IsNullOrEmpty(txtName.Text) &&
                   !string.IsNullOrEmpty(txtFamily.Text) &&
                   !string.IsNullOrEmpty(txtPassword.Password) &&
                   !string.IsNullOrEmpty(txtAge.Text);
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login(_sharedViewModel));
        }

        private void txtAge_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnRegister_Click(sender, e);
            }
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                {
                    parentWindow.DragMove();
                }

            }
        }
    }
}
