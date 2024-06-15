using FinalProjectRoniel.ViewModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace FinalProjectRoniel.View
{
    public partial class MainWithFrame : Window
    {
        private SharedViewModel viewModel;
        private int count = 0;
        public MainWithFrame()
        {
            InitializeComponent();
            viewModel = new SharedViewModel();
            mainFrame.Navigate(new Login(viewModel));
            Application.Current.Windows[0].Height = 900;
            Application.Current.Windows[0].Width = 1000;
        }
    }
}
