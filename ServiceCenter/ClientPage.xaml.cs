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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ServiceCenter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClientPage : Page
    {
        public int clientID = ClientAdminWindow.userID;
        public ClientPage()
        {
            this.InitializeComponent();
            using (SqlConnection connection = new SqlConnection(@"Data Source=BALABOLKIN-DESK\SQLEXPRESS;Initial Catalog=ServiceCenter;Integrated Security=True;"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT FullName from Clients where IDClient=@id;", connection);
                SqlParameter parameter = new SqlParameter("@id", clientID);
                command.Parameters.Add(parameter);
                SqlDataReader sqlDataReader = command.ExecuteReader();
                sqlDataReader.Read();
                headerText.Text = "Здравствуйте, "+sqlDataReader.GetString(0);
            }
        }
    }
}
