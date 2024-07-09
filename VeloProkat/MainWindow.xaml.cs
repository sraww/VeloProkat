using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VeloProkat.Infrastructure;
using VeloProkat.Models;

namespace VeloProkat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool verify = true;
        int verifyCheck = 0;
        public MainWindow()
        {
            InitializeComponent();
            captchaBox.Visibility = Visibility.Collapsed;
            captchaBox.Visibility = Visibility.Collapsed;

            MessageBox.Show("Добро пожаловать!");
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            using (VeloProkatContext db = new VeloProkatContext())
            {

                // проверка, если есть каптча
                if (captchaBox.Visibility == Visibility.Visible)
                {
                    if (captchaBox.Text == captchaBox.Text)
                    {
                        verify = true;
                    }
                }

                User user = db.Users.Where(u => u.Login == textBox_Login.Text && u.Password == passwordBox_Password.Password).Include(u => u.RoleNavigation).FirstOrDefault() as User;

                // admin
                if (user != null && verify)
                {
                    new Window_info(user).Show();
                    
                }
                else
                {
                    MessageBox.Show("Неуспешная авторизация");
                    verifyCheck += 1;

                    // captcha view
                    captchaBox.Visibility = Visibility.Visible;
                    captchaBox.Visibility = Visibility.Visible;
                    captchaBox.Text = CaptchaBuilder.Refresh();
                    verify = false;

                    if (verifyCheck > 1)
                    {
                        disableButton();
                        captchaBox.Text = CaptchaBuilder.Refresh();
                    }
                }
            }

        }

        /// <summary>
        /// Асинхронное выключение кнопки на 10 сек.
        /// </summary>
        async void disableButton()
        {
            loginButton.IsEnabled = false;
            await Task.Delay(TimeSpan.FromSeconds(15));
            loginButton.IsEnabled = true;
        }

        private void captchaBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ButtonGuest_Click(object sender, RoutedEventArgs e)
        {
            new Window_info(null).Show();
            this.Close();

        }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
