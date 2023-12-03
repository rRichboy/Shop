using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Diagnostics;
using Test.Models;
using Test.ViewModels;

namespace Test.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            DataContext = viewModel;
        }

        public MainWindow(bool isAdmin)
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            DataContext = viewModel;
            DG.IsVisible = true;
            Admin.IsVisible = true;
            
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SaveDataToDatabaseAsync(id, pr_name, desc, price);
        }

        public void Button_Click1(object sender, RoutedEventArgs e)
        {
            viewModel.DeleteData(int.Parse(dell_id.Text));
        }

        private void ButtonSpinner_Spin(object? sender, Avalonia.Controls.SpinEventArgs e)
        {
            ButtonSpinner spinner = sender as ButtonSpinner;

            int value = Convert.ToInt32(spinner.Content);

            if (e.Direction == SpinDirection.Increase)
                value++;
            else if (e.Direction == SpinDirection.Decrease && value == 0)
                return;
            else
                value--;

            spinner.Content = value;

            var SelectedItem = DGs.SelectedItem as Product;

            (DataContext as MainWindowViewModel).CartList.Add(SelectedItem);

        }

        private void Button_Click2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Cart.IsVisible = true;
            
            var SelectedItem = DGs.SelectedItem as Product;

            (DataContext as MainWindowViewModel).CartList.Add(SelectedItem);

            viewModel.SelectData(id, pr_name, desc, price);
        }

        private void Button_Click3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (GridCart.Columns.Count > 0) GridCart.IsVisible = true;

            var SelectedItem = GridCart.SelectedItem as Product;

            (DataContext as MainWindowViewModel).CartList.Remove(SelectedItem);

        }

        private void Button_Click4(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            //
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedOption = (myComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selectedOption == "Добавить")
            {
                add.IsVisible = true; 
                del.IsVisible = false;
            }
            else if (selectedOption == "Удалить")
            {
                add.IsVisible = false;
                del.IsVisible = true;
            }
        }
    }
}
