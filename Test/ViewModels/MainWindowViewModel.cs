using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Test.Context;
using Test.Models;

namespace Test.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Product> Products { get; set; }

        public ObservableCollection<Product> CartList { get; set; }
        public object SelectedQuantities { get; internal set; }

        public TextBox pr_name;
        public TextBox desc;
        public TextBox price;
        public TextBox pr_name1;
        public TextBlock Errr;


        public MainWindowViewModel()
        {
            CartList = new ObservableCollection<Product>();

            InitializeData();

        }

        public void InitializeData()
        {
            using (var context = new TestContext())
            {
                Products = new ObservableCollection<Product>(context.Products);
            }
        }

        //добавить в бд
        public async Task SaveDataToDatabaseAsync(TextBox pr_name, TextBox desc, TextBox price, TextBlock Errr)
        {
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
                        cmd.CommandText = "INSERT INTO products (product_name, description, price) VALUES (@productName, @description, @price)";

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

                pr_name.Text = string.Empty;
                desc.Text = string.Empty;
                price.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Errr.Text = "Товар с таким именем уже существует";
            }

        }

        //удалить с бд
        public async Task DeleteData(TextBox pr_name1,TextBlock Errr)
        {
            try
            {
                using (var connection = new NpgsqlConnection("Host=localhost;Database=Test;Username=postgres;Password=admin"))
                {
                    await connection.OpenAsync();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "DELETE FROM products WHERE product_name = @product_name";

                        cmd.Parameters.AddWithValue("product_name", pr_name1.Text);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                using (var context = new TestContext())
                {
                    Products = new ObservableCollection<Product>(context.Products);
                }

                pr_name1.Text = string.Empty;
            }

            catch (Exception ex)
            {
                Errr.Text = "Товар с таким именем не существует";
            }

        }

        //выбор+добавить в корзину
        public async Task SelectData(TextBox pr_name, TextBox desc, TextBox price)
        {
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
                        cmd.CommandText = "SELECT * FROM products WHERE product_name = @product_name";

                        cmd.Parameters.AddWithValue("product_name", pr_name);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                using (var context = new TestContext())
                {
                    Products = new ObservableCollection<Product>(context.Products);
                }
               
            }

            catch (Exception ex)
            {

            }
        }
    }
}