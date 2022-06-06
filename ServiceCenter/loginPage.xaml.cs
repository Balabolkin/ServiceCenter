using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ServiceCenter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class loginPage : Page
    {
        public loginPage()
        {
            this.InitializeComponent();
        }
        private void userTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (userTypeCombobox.SelectedIndex == 1)
                signupButton.IsEnabled = false;
            if (userTypeCombobox.SelectedIndex == 0)
                signupButton.IsEnabled = true;
        }

        private void signupButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(signupPage));
        }

        private void signinButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loginTextbox.Text) || string.IsNullOrEmpty(passwordbox.Password))
            {
                flyoutTextBlock.Text = "Заполните все поля!";
                Flyout.ShowAttachedFlyout(signinButton);
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=BALABOLKIN-DESK\SQLEXPRESS;Initial Catalog=ServiceCenter;Integrated Security=True;"))
                {
                    connection.Open();
                    SqlCommand command;
                    if (userTypeCombobox.SelectedIndex == 1)
                        command = new SqlCommand("SELECT IDEmployee from Employees where Login=@login AND Password=@passw;", connection);
                    else
                        command = new SqlCommand("SELECT IDClient from Clients where Login=@login AND Password=@passw;", connection);
                    SqlParameter logPr = new SqlParameter("@login", loginTextbox.Text);
                    command.Parameters.Add(logPr);
                    SqlParameter PassPr = new SqlParameter("@passw", passwordbox.Password);
                    command.Parameters.Add(PassPr);
                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.Read())
                    {
                        ClientAdminWindow window = new ClientAdminWindow(sqlDataReader.GetInt32(0), userTypeCombobox.SelectedIndex);
                        window.Show();
                        App.m_window.Hide();
                    }
                    else
                    {
                        flyoutTextBlock.Text = "Неверный логин или пароль!";
                        Flyout.ShowAttachedFlyout(signinButton);
                    }
                }
            }
        }
    }
}
