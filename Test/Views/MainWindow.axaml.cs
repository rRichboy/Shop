using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace Test.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public async void Button_Click(object sender, RoutedEventArgs e)
        {
            await SaveDataToDatabaseAsync();
        }
    }
}