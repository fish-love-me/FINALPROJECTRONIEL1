using FinalProjectRoniel.Model;
using FinalProjectRoniel.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FinalProjectRoniel.View
{
    public partial class Login : Page
    {
        private SharedViewModel _sharedViewModel;
        private bool max = false;

        public Login(SharedViewModel sharedViewModel)
        {
            InitializeComponent();
            _sharedViewModel = sharedViewModel;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!AnimationTracker.HasPlayed)
            {
                AnimationTracker.HasPlayed = true;

                Storyboard fadeInStoryboard = (Storyboard)this.Resources["FadeInStoryboard"];
                fadeInStoryboard.Begin(this);

                MediaElement startSound = (MediaElement)this.FindName("StartSound");
                startSound.Play();
            }
            else
            {
                this.Opacity = 1;
            }
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (max == false)
            {
                parentWindow.WindowState = WindowState.Maximized;
                max = true;
            }
            else
            {
                parentWindow.WindowState = WindowState.Normal;
                max = false;
            }
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string enteredEmail = txtUser.Text;
                string enteredPassword = txtPassword.Password;

                string connectionString = @"Data Source=C:\Users\ronie\source\repos\mvvm\project_machshevon\table1.db;Version=3;";
                bool userFound = false;
                bool isAdmin = false;

                using (var connection = new System.Data.SQLite.SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password LIMIT 1";
                    using (var command = new System.Data.SQLite.SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", enteredEmail);
                        command.Parameters.AddWithValue("@Password", enteredPassword);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userFound = true;
                                isAdmin = Convert.ToBoolean(reader["IsAdmin"]);
                            }
                        }
                    }
                }

                // Check if a match was found
                if (userFound)
                {
                    // User found, create and show the MainWithFrame window
                    MainWithFrame mainWithFrame = new MainWithFrame();
                    mainWithFrame.Show();

                    // Now, navigate the Frame within MainWithFrame to MainPage or AdminPage based on isAdmin flag
                    if (isAdmin)
                    {
                        AdminDashboardPage adminDashboardPage = new AdminDashboardPage(_sharedViewModel);
                        mainWithFrame.mainFrame.Navigate(adminDashboardPage);
                    }
                    else
                    {
                        Trivia trivia = new Trivia(_sharedViewModel);
                        mainWithFrame.mainFrame.Navigate(trivia);
                    }

                    Window.GetWindow(this).Close();
                }
                else
                {
                    // No match found, display an error message
                    MessageBox.Show("Invalid email or password. Please try again.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Register(_sharedViewModel));
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(sender, e);
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
