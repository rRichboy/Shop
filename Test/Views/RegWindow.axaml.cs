using Avalonia.Controls;
using Avalonia.Interactivity;
using Test.ViewModels;

namespace Test.Views
{
    public partial class RegWindow : Window
    {
        public bool IsAdmin;
        public RegWindow()
        {
            InitializeComponent();
        }

        private void Signin_click(object sender, RoutedEventArgs e)
        {
            if (CodeTextBox.Text == "0000")
            {
                IsAdmin = true;
                var win = new MainWindow(IsAdmin);
                win.DataContext = new MainWindowViewModel();
                win.Show();

            }
            else
            {
                var win = new MainWindow();
                win.DataContext = new MainWindowViewModel();
                win.Show();

            }

            this.Close();

        }
    }
}
