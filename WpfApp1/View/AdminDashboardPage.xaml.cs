using FinalProjectRoniel.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinalProjectRoniel.View
{
    public partial class AdminDashboardPage : Page
    {
        private SharedViewModel _sharedViewModel;

        public AdminDashboardPage(SharedViewModel sharedViewModel)
        {
            InitializeComponent();
            _sharedViewModel = sharedViewModel;
        }

        private void ManageUsers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new ManageTablePage("Users"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to Manage Users: {ex.Message}");
            }
        }

        private void ManageQuestions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new ManageTablePage("Questions"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to Manage Questions: {ex.Message}");
            }
        }

        private void ManageAnswers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new ManageTablePage("Answers"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to Manage Answers: {ex.Message}");
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow.WindowState == WindowState.Maximized)
            {
                parentWindow.WindowState = WindowState.Normal;
            }
            else
            {
                parentWindow.WindowState = WindowState.Maximized;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login(_sharedViewModel)); 
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
