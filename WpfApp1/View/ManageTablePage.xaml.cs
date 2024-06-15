using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinalProjectRoniel.View
{
    public partial class ManageTablePage : Page
    {
        private string _tableName;
        private string connectionString = @"Data Source=C:\Users\ronie\source\repos\mvvm\project_machshevon\table1.db;Version=3;";

        public ManageTablePage(string tableName)
        {
            InitializeComponent();
            _tableName = tableName;
            DataContext = this;
            LoadTable();
        }

        public string TableName => $"Manage {_tableName}";

        private void LoadTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM {_tableName}";
                    using (var adapter = new SQLiteDataAdapter(sql, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGrid.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading table: {ex.Message}");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = "";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        if (_tableName == "Users")
                        {
                            string email = PromptDialog.ShowDialog("Enter Email:", "Add User");
                            string password = PromptDialog.ShowDialog("Enter Password:", "Add User");
                            string firstName = PromptDialog.ShowDialog("Enter First Name:", "Add User");
                            string lastName = PromptDialog.ShowDialog("Enter Last Name:", "Add User");
                            string age = PromptDialog.ShowDialog("Enter Age:", "Add User");

                            sql = "INSERT INTO Users (Email, Password, FirstName, LastName, Age, IsAdmin) VALUES (@Email, @Password, @FirstName, @LastName, @Age, 0)";
                            command.CommandText = sql;
                            command.Parameters.AddWithValue("@Email", email);
                            command.Parameters.AddWithValue("@Password", password);
                            command.Parameters.AddWithValue("@FirstName", firstName);
                            command.Parameters.AddWithValue("@LastName", lastName);
                            command.Parameters.AddWithValue("@Age", int.Parse(age));
                        }
                        else if (_tableName == "Questions")
                        {
                            string questionText = PromptDialog.ShowDialog("Enter Question:", "Add Question");

                            sql = "INSERT INTO Questions (QuestionText) VALUES (@QuestionText)";
                            command.CommandText = sql;
                            command.Parameters.AddWithValue("@QuestionText", questionText);
                        }
                        else if (_tableName == "Answers")
                        {
                            string questionId = PromptDialog.ShowDialog("Enter Question ID:", "Add Answer");
                            string answerText = PromptDialog.ShowDialog("Enter Answer Text:", "Add Answer");
                            string isCorrect = PromptDialog.ShowDialog("Is Correct (0 or 1):", "Add Answer");

                            sql = "INSERT INTO Answers (QuestionId, AnswerText, IsCorrect) VALUES (@QuestionId, @AnswerText, @IsCorrect)";
                            command.CommandText = sql;
                            command.Parameters.AddWithValue("@QuestionId", int.Parse(questionId));
                            command.Parameters.AddWithValue("@AnswerText", answerText);
                            command.Parameters.AddWithValue("@IsCorrect", int.Parse(isCorrect));
                        }

                        command.ExecuteNonQuery();
                    }
                }
                LoadTable();
                MessageBox.Show("Record added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding record: {ex.Message}");
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGrid.SelectedItem is DataRowView rowView)
                {
                    DataRow row = rowView.Row;
                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "";
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            if (_tableName == "Users")
                            {
                                string userId = row["UserId"].ToString();
                                string email = PromptDialog.ShowDialog("Edit Email:", "Edit User", row["Email"].ToString());
                                string password = PromptDialog.ShowDialog("Edit Password:", "Edit User", row["Password"].ToString());
                                string firstName = PromptDialog.ShowDialog("Edit First Name:", "Edit User", row["FirstName"].ToString());
                                string lastName = PromptDialog.ShowDialog("Edit Last Name:", "Edit User", row["LastName"].ToString());
                                string age = PromptDialog.ShowDialog("Edit Age:", "Edit User", row["Age"].ToString());

                                sql = "UPDATE Users SET Email = @Email, Password = @Password, FirstName = @FirstName, LastName = @LastName, Age = @Age WHERE UserId = @UserId";
                                command.CommandText = sql;
                                command.Parameters.AddWithValue("@UserId", int.Parse(userId));
                                command.Parameters.AddWithValue("@Email", email);
                                command.Parameters.AddWithValue("@Password", password);
                                command.Parameters.AddWithValue("@FirstName", firstName);
                                command.Parameters.AddWithValue("@LastName", lastName);
                                command.Parameters.AddWithValue("@Age", int.Parse(age));
                            }
                            else if (_tableName == "Questions")
                            {
                                string questionId = row["QuestionId"].ToString();
                                string questionText = PromptDialog.ShowDialog("Edit Question:", "Edit Question", row["QuestionText"].ToString());

                                sql = "UPDATE Questions SET QuestionText = @QuestionText WHERE QuestionId = @QuestionId";
                                command.CommandText = sql;
                                command.Parameters.AddWithValue("@QuestionId", int.Parse(questionId));
                                command.Parameters.AddWithValue("@QuestionText", questionText);
                            }
                            else if (_tableName == "Answers")
                            {
                                string answerId = row["AnswerId"].ToString();
                                string questionId = PromptDialog.ShowDialog("Edit Question ID:", "Edit Answer", row["QuestionId"].ToString());
                                string answerText = PromptDialog.ShowDialog("Edit Answer Text:", "Edit Answer", row["AnswerText"].ToString());
                                string isCorrect = PromptDialog.ShowDialog("Edit Is Correct (0 or 1):", "Edit Answer", row["IsCorrect"].ToString());

                                sql = "UPDATE Answers SET QuestionId = @QuestionId, AnswerText = @AnswerText, IsCorrect = @IsCorrect WHERE AnswerId = @AnswerId";
                                command.CommandText = sql;
                                command.Parameters.AddWithValue("@AnswerId", int.Parse(answerId));
                                command.Parameters.AddWithValue("@QuestionId", int.Parse(questionId));
                                command.Parameters.AddWithValue("@AnswerText", answerText);
                                command.Parameters.AddWithValue("@IsCorrect", int.Parse(isCorrect));
                            }

                            command.ExecuteNonQuery();
                        }
                    }
                    LoadTable();
                    MessageBox.Show("Record updated successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing record: {ex.Message}");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGrid.SelectedItem is DataRowView rowView)
                {
                    DataRow row = rowView.Row;
                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "";
                        using (var command = new SQLiteCommand(sql, connection))
                        {
                            if (_tableName == "Users")
                            {
                                sql = "DELETE FROM Users WHERE UserId = @UserId";
                                command.CommandText = sql;
                                command.Parameters.AddWithValue("@UserId", row["UserId"]);
                            }
                            else if (_tableName == "Questions")
                            {
                                sql = "DELETE FROM Questions WHERE QuestionId = @QuestionId";
                                command.CommandText = sql;
                                command.Parameters.AddWithValue("@QuestionId", row["QuestionId"]);
                            }
                            else if (_tableName == "Answers")
                            {
                                sql = "DELETE FROM Answers WHERE AnswerId = @AnswerId";
                                command.CommandText = sql;
                                command.Parameters.AddWithValue("@AnswerId", row["AnswerId"]);
                            }

                            command.ExecuteNonQuery();
                        }
                    }
                    LoadTable();
                    MessageBox.Show("Record deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting record: {ex.Message}");
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
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

    public static class PromptDialog
    {
        public static string ShowDialog(string text, string caption, string defaultValue = "")
        {
            Window prompt = new Window
            {
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = caption
            };

            StackPanel stackPanel = new StackPanel();

            TextBlock textBlock = new TextBlock { Text = text, Margin = new Thickness(10) };
            stackPanel.Children.Add(textBlock);

            TextBox textBox = new TextBox { Text = defaultValue, Margin = new Thickness(10) };
            stackPanel.Children.Add(textBox);

            Button confirmation = new Button { Content = "Ok", Width = 50, Margin = new Thickness(10) };
            confirmation.Click += (sender, e) => { prompt.DialogResult = true; prompt.Close(); };
            stackPanel.Children.Add(confirmation);

            prompt.Content = stackPanel;
            prompt.ShowDialog();

            return textBox.Text;
        }
    }
}
