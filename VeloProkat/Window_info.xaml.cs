using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VeloProkat.Models;

namespace VeloProkat
{
    /// <summary>
    /// Логика взаимодействия для Window_info.xaml
    /// </summary>
    public partial class Window_info : Window
    {
        public Window_info(User user)
        {
            InitializeComponent();

            using (VeloProkatContext db = new VeloProkatContext())
            {
                countProducts.Text = $"Количество: {db.Products.Count()}";

                List<string> sortList = new List<string>() { "По возрастанию цены", "По убыванию цены" };
                List<string> filtertList = db.Products.Select(u => u.Manufacturer).Distinct().ToList();
                filtertList.Insert(0, "Все производители");
                sortUserComboBox.ItemsSource = sortList.ToList();
                filterUserComboBox.ItemsSource = filtertList.ToList();
                if (user != null)
                {
                    if (user.RoleNavigation.Name == "Администратор")
                    {
                        deleteUser.Visibility = Visibility.Visible;
                        addUser.Visibility = Visibility.Visible;
                    }
                    statusUser.Text = $"{user.RoleNavigation.Name}:{user.Surname}:{user.Name}:{user.Patronymic}";
                    MessageBox.Show($"{user.RoleNavigation.Name}: {user.Surname} {user.Name} {user.Patronymic}. \r\t");
                }
                if (user == null)
                {
                    statusUser.Text = "Гость";
                    deleteUser.Visibility = Visibility.Collapsed;
                    addUser.Visibility = Visibility.Collapsed;

                }
                else
                {

                  statusUser.Text = "Гость";
                   MessageBox.Show("Гость");
                }


                productlistView.ItemsSource = db.Products.ToList();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
        private void exitButtonClick(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void sortUserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         UpdateProducts();
        }

        private void filterUserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void UpdateProducts()
        {
            using (VeloProkatContext db = new VeloProkatContext())
            {
                var currentProducts = db.Products.ToList();
                productlistView.ItemsSource = currentProducts;

                //Сортировка
                if (sortUserComboBox.SelectedIndex != -1)
                {
                    if (sortUserComboBox.SelectedValue == "По убыванию цены")
                    {
                        currentProducts = currentProducts.OrderByDescending(u => u.Cost).ToList();
                    }

                    if (sortUserComboBox.SelectedValue == "По возрастанию цены")
                    {
                        currentProducts = currentProducts.OrderBy(u => u.Cost).ToList();
                    }
                }


                // Фильтрация
                if (filterUserComboBox.SelectedIndex != -1)
                {
                    if (db.Products.Select(u => u.Manufacturer).Distinct().ToList().Contains(filterUserComboBox.SelectedValue))
                    {
                        currentProducts = currentProducts.Where(u => u.Manufacturer == filterUserComboBox.SelectedValue.ToString()).ToList();
                    }
                    else
                    {
                        currentProducts = currentProducts.ToList();
                    }
                }

                // Поиск

                if (searchBox.Text.Length > 0)
                {
                    currentProducts = currentProducts.Where(u => u.Name.Contains(searchBox.Text) || u.Description.Contains(searchBox.Text)).ToList();
                }
                countProducts.Text = $"Количество: {currentProducts.Count} из {db.Products.ToList().Count}";
                productlistView.ItemsSource = currentProducts;
            }
        }

        private void сlearButton_Click(object sender, RoutedEventArgs e)
        {
            searchBox.Text = "";
            sortUserComboBox.SelectedIndex = -1;
            filterUserComboBox.SelectedIndex = -1;
        }

        private void addUserButtonClick(object sender, RoutedEventArgs e)
        {
            new AddProductWindow(null).ShowDialog();
        }

        private void delUserButton(object sender, RoutedEventArgs e)
        {

            using (VeloProkatContext db = new VeloProkatContext())
            {
                var product = (productlistView.SelectedItem) as Product;

                if (product != null)
                {

                    if (MessageBox.Show($"Вы точно хотите удалить {product.Name}", "Внимание!",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        db.Products.Remove(product);
                        db.SaveChanges();
                        MessageBox.Show($"Товар {product.Name} удален!");
                        productlistView.ItemsSource = db.Products.ToList();
                        countProducts.Text = $"Количество: {db.Products.Count()}";
                    }

                }
            }

        }
        private bool CanDeleteProduct(Product product)
        {
            using (VeloProkatContext db = new VeloProkatContext())
            {

           //  List<RelatedProduct> rp = db.RelatedProducts.Where(p => p.ProductId == product.Id).ToList();
              foreach (RelatedProduct r in r.p)
              {
                   OrderProduct order = db.OrderProducts.Where(o => o.ProductId == r.RelatedProdutId).FirstOrDefault() as OrderProduct;
                 if (order is not null)
                  {
                       Product p = db.Products.Where(p => p.Id == r.RelatedProdutId).FirstOrDefault() as Product;
                      MessageBox.Show($"Товар {p.Name} связан с товаром {product.Name} присутствует в товарной позиции заказа {order.OrderId}. \n Товары нельзя удалить!");
                        return false;
                   }
               }
              return true;
              OrderProduct position = db.OrderProducts.Where(o => o.ProductId == product.Id).FirstOrDefault() as OrderProduct;
                if (position is not null)
                {
                   MessageBox.Show($"Товар: {product.Name} присутствует в товарной позиции заказа {position.OrderId}. \n Товар нельзя удалить!");
                    return false;
               }
                return true;
           }
        }
        private void EditProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Product p = (sender as ListView).SelectedItem as Product;
            new AddProductWindow(p).ShowDialog();
        }
    }
}


        
      
