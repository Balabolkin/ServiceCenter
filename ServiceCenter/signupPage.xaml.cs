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
    public sealed partial class signupPage : Page
    {
        public signupPage()
        {
            this.InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(loginPage));
        }

        private void signupButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loginTextbox.Text) || string.IsNullOrEmpty(passwordbox.Password) || string.IsNullOrEmpty(fullnameTextbox.Text))
            {
                flyoutTextBlock.Text = "Заполните все поля!";
                Flyout.ShowAttachedFlyout(signupButton);
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=BALABOLKIN-DESK\SQLEXPRESS;Initial Catalog=ServiceCenter;Integrated Security=True;"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO Clients (FullName, Login, Password) VALUES  (@fullname, @login, @pass); SELECT SCOPE_IDENTITY();", connection);
                    SqlParameter fnPr = new SqlParameter("@fullname", fullnameTextbox.Text);
                    command.Parameters.Add(fnPr);
                    SqlParameter logPr = new SqlParameter("@login", loginTextbox.Text);
                    command.Parameters.Add(logPr);
                    SqlParameter PassPr = new SqlParameter("@pass", passwordbox.Password);
                    command.Parameters.Add(PassPr);
                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    sqlDataReader.Read();
                    ClientAdminWindow window = new ClientAdminWindow(Decimal.ToInt32(sqlDataReader.GetDecimal(0)), 0);
                    window.Show();
                    App.m_window.Hide();
                }
            }
        }
    }
}
