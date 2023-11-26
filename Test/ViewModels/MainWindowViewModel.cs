using Avalonia.Controls;
using Avalonia.Interactivity;
using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Test.Context;
using Test.Models;

namespace Test.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Product> Products { get; set; }
        public TextBox id;
        public TextBox pr_name;
        public TextBox desc;
        public TextBox price;

        public MainWindowViewModel()
        {
            InitializeData();
        }

        public void InitializeData()
        {
            using (var context = new TestContext())
            {
                Products = new ObservableCollection<Product>(context.Products);
            }
        }

        private async Task SaveDataToDatabaseAsync()
        {
            int.TryParse(id.Text, out int productId);
            string productName = pr_name.Text;
            string description = desc.Text;
            decimal.TryParse(price.Text, out decimal productPrice);

            try
            {
                using (var connection = new NpgsqlConnection("Host=localhost;Database=Test;Username=postgres;Password=admin"))
                {
                    await connection.OpenAsync();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "INSERT INTO products (id, product_name, description, price) VALUES (@id, @productName, @description, @price)";

                        cmd.Parameters.AddWithValue("id", productId);
                        cmd.Parameters.AddWithValue("productName", productName);
                        cmd.Parameters.AddWithValue("description", description);
                        cmd.Parameters.AddWithValue("price", productPrice);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                using (var context = new TestContext())
                {
                    Products = new ObservableCollection<Product>(context.Products);
                }

                ClearTextBoxes();
            }
            catch (Exception ex)
            {

            }
        }

        private void ClearTextBoxes()
        {
            id.Text = string.Empty;
            pr_name.Text = string.Empty;
            desc.Text = string.Empty;
            price.Text = string.Empty;
        }
    }
}