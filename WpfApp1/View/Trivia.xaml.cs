using FinalProjectRoniel.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProjectRoniel.View
{
    public partial class Trivia : Page
    {
        private SharedViewModel _sharedViewModel;
        private List<int> _questionIds;
        private int _currentQuestionIndex;
        private int _score;
        private int _totalQuestions;
        private MediaPlayer _mediaPlayer;

        public Trivia(SharedViewModel sharedViewModel)
        {
            InitializeComponent();
            _sharedViewModel = sharedViewModel;
            _questionIds = new List<int>();
            _currentQuestionIndex = -1;
            _score = 0;
            _totalQuestions = 10; // Set total questions to 10
            _mediaPlayer = new MediaPlayer();
            LoadQuestions();
            LoadNextQuestion();
        }

        private void LoadQuestions()
        {
            try
            {
                string connectionString = @"Data Source=C:\Users\ronie\source\repos\mvvm\project_machshevon\table1.db;Version=3;";
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT QuestionId FROM Questions";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _questionIds.Add(Convert.ToInt32(reader["QuestionId"]));
                            }
                        }
                    }

                    if (_questionIds.Count == 0)
                    {
                        MessageBox.Show("No questions available in the database.");
                        NavigationService.Navigate(new Login(_sharedViewModel));
                    }
                    else
                    {
                        // Shuffle the questions to randomize their order
                        _questionIds = _questionIds.OrderBy(x => Guid.NewGuid()).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading questions: {ex.Message}");
            }
        }

        private void LoadNextQuestion()
        {
            if (_currentQuestionIndex + 1 >= _totalQuestions || _questionIds.Count == 0)
            {
                MessageBox.Show($"Game over! Your score: {_score}");
                NavigationService.Navigate(new Login(_sharedViewModel));
                return;
            }

            _currentQuestionIndex++;
            QuestionsLeft.Text = $"QUESTIONS LEFT: {_totalQuestions - _currentQuestionIndex}";
            LoadRandomQuestion();
        }

        private void LoadRandomQuestion()
        {
            try
            {
                int questionId = _questionIds[_currentQuestionIndex];
                string connectionString = @"Data Source=C:\Users\ronie\source\repos\mvvm\project_machshevon\table1.db;Version=3;";
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Questions WHERE QuestionId = @QuestionId";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@QuestionId", questionId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                QuestionText.Text = reader["QuestionText"].ToString();
                            }
                        }
                    }

                    sql = "SELECT * FROM Answers WHERE QuestionId = @QuestionId";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@QuestionId", questionId);
                        using (var reader = command.ExecuteReader())
                        {
                            List<(string AnswerText, bool IsCorrect)> answers = new List<(string, bool)>();
                            while (reader.Read())
                            {
                                answers.Add((reader["AnswerText"].ToString(), Convert.ToBoolean(reader["IsCorrect"])));
                            }

                            // Shuffle the answers to randomize their order
                            answers = answers.OrderBy(a => Guid.NewGuid()).ToList();

                            // Assign shuffled answers to radio buttons
                            AnswerButton1.Content = answers[0].AnswerText;
                            AnswerButton1.Tag = answers[0].IsCorrect;

                            AnswerButton2.Content = answers[1].AnswerText;
                            AnswerButton2.Tag = answers[1].IsCorrect;

                            AnswerButton3.Content = answers[2].AnswerText;
                            AnswerButton3.Tag = answers[2].IsCorrect;

                            AnswerButton4.Content = answers[3].AnswerText;
                            AnswerButton4.Tag = answers[3].IsCorrect;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the question: {ex.Message}");
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton selectedButton = null;
            if (AnswerButton1.IsChecked == true) selectedButton = AnswerButton1;
            if (AnswerButton2.IsChecked == true) selectedButton = AnswerButton2;
            if (AnswerButton3.IsChecked == true) selectedButton = AnswerButton3;
            if (AnswerButton4.IsChecked == true) selectedButton = AnswerButton4;

            if (selectedButton != null && selectedButton.Tag is bool isCorrect)
            {
                if (isCorrect)
                {
                    _score++;
                    _mediaPlayer.Open(new Uri("pack://siteoforigin:,,,/Resources/correct.wav"));
                }
                else
                {
                    _mediaPlayer.Open(new Uri("pack://siteoforigin:,,,/Resources/wrong.wav"));
                }
                _mediaPlayer.Play();
                LoadNextQuestion();
            }
            else
            {
                MessageBox.Show("Please select an answer.");
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.WindowState = WindowState.Minimized;
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

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            // Logic to handle answer click if needed
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
