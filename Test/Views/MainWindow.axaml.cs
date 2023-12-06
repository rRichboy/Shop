using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Test.Models;
using Test.ViewModels;

namespace Test.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel;

        private List<Product> selectedProducts = new List<Product>();

        private decimal totalAmount = 0;

        //private void Initialize()
        //{
        //    totalAmountTextBlock = this.FindControl<TextBlock>("TotalAmountTextBlock");
        //}


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
            viewModel.SaveDataToDatabaseAsync(pr_name, desc, price,Errr);
        }

        public void Button_Click1(object sender, RoutedEventArgs e)
        {
            viewModel.DeleteData(pr_name1, Errr);
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

            var selectedProduct = DGs.SelectedItem as Product;
            if (selectedProduct != null)
            {
                selectedProduct.Quantity = value;

                if (!selectedProducts.Contains(selectedProduct))
                {
                    selectedProducts.Add(selectedProduct);
                }
            }
        }

        private void Button_Click2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Cart.IsVisible = true;

            foreach (var selectedProduct in selectedProducts)
            {
                var existingProduct = (DataContext as MainWindowViewModel).CartList.FirstOrDefault(p => p.Id == selectedProduct.Id);
                if (existingProduct != null)
                {
                    existingProduct.Quantity += selectedProduct.Quantity;
                }
                else
                {
                    var newProduct = new Product
                    {
                        Id = selectedProduct.Id,
                        ProductName = selectedProduct.ProductName,
                        Description = selectedProduct.Description,
                        Price = selectedProduct.Price,
                        Quantity = selectedProduct.Quantity
                    };
                    (DataContext as MainWindowViewModel).CartList.Add(newProduct);

                    UpdateTotalAmount();

                }
            }

            selectedProducts.Clear();

            viewModel.SelectData(pr_name, desc, price);

        }

        private void UpdateTotalAmount()
        {
            totalAmount = 0;

            foreach (var selectedProduct in (DataContext as MainWindowViewModel).CartList)
            {
                totalAmount += selectedProduct.Price * selectedProduct.Quantity;
            }

            totalAmountTextBlock.Text = $"Общая сумма в корзине: {totalAmount:C}";
        }

       
        private void Button_Click3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (GridCart.Columns.Count > 0) GridCart.IsVisible = true;

            var SelectedItem = GridCart.SelectedItem as Product;

            (DataContext as MainWindowViewModel).CartList.Remove(SelectedItem);

            UpdateTotalAmount();

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedOption = (myComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            Errr.Text = string.Empty;

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
