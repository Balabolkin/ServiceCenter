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
    public sealed partial class AdminPage : Page
    {
        public int employeeID = ClientAdminWindow.userID;
        public AdminPage()
        {
            this.InitializeComponent();
        }

        private string getClientName(int idclient)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=BALABOLKIN-DESK\SQLEXPRESS;Initial Catalog=ServiceCenter;Integrated Security=True;"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT FullName from Clients where IDClient=@id;", connection);
                SqlParameter parameter = new SqlParameter("@id", idclient);
                command.Parameters.Add(parameter);
                SqlDataReader sqlDataReader = command.ExecuteReader();
                sqlDataReader.Read();
                return sqlDataReader.GetString(0);
            }
        }

        async private void RequestListViewFill()
        {
            await using (SqlConnection connection = new SqlConnection(@"Data Source=BALABOLKIN-DESK\SQLEXPRESS;Initial Catalog=ServiceCenter;Integrated Security=True;"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Status, RequestDate, IDClient, IDEmployee from Requests where IDEmployee=@id;", connection);
                SqlParameter parameter = new SqlParameter("@id", employeeID);
                command.Parameters.Add(parameter);
                SqlDataReader sqlDataReader = command.ExecuteReader();
                while(sqlDataReader.Read())
                {
                    RequestListView.Items.Add(new RequestList(getClientName(sqlDataReader.GetInt32(2)), sqlDataReader.GetString(0), sqlDataReader.GetDateTime(1).ToString(), sqlDataReader.GetInt32(3)));
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=BALABOLKIN-DESK\SQLEXPRESS;Initial Catalog=ServiceCenter;Integrated Security=True;"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT FullName from Employees where IDEmployee=@id;", connection);
                SqlParameter parameter = new SqlParameter("@id", employeeID);
                command.Parameters.Add(parameter);
                SqlDataReader sqlDataReader = command.ExecuteReader();
                sqlDataReader.Read();
                headerText.Text = "Здравствуйте, " + sqlDataReader.GetString(0);
            }
            RequestListViewFill();
        }

        private void RequestListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ServiceListView.Visibility = Visibility.Visible;
            RequestList requestList = (RequestList)e.AddedItems.First();
            using (SqlConnection connection = new SqlConnection(@"Data Source=BALABOLKIN-DESK\SQLEXPRESS;Initial Catalog=ServiceCenter;Integrated Security=True;"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT ServiceName, Price from CompServices JOIN RequestService ON CompServices.IDService=RequestService.IDService;", connection);
                SqlDataReader sqlDataReader = command.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    ServiceListView.Items.Add(new ServicesList(sqlDataReader.GetString(0), sqlDataReader.GetSqlMoney(1).ToString() + " р."));
                }
            }
            using (SqlConnection connection = new SqlConnection(@"Data Source=BALABOLKIN-DESK\SQLEXPRESS;Initial Catalog=ServiceCenter;Integrated Security=True;"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Description from Requests WHERE IDRequest=@id;", connection);
                SqlParameter parameter = new SqlParameter("@id", requestList.id);
                command.Parameters.Add(parameter);
                SqlDataReader sqlDataReader = command.ExecuteReader();
                sqlDataReader.Read();
                descriptionTextblock.Text = "Описание: "+sqlDataReader.GetString(0);
            }
        }
    }
    public class RequestList
    {
        public string clientFullname { get; private set; }
        public string state { get; private set; }
        public string dateTime { get; private set; }
        public int id { get; private set; }
        public RequestList(string fullname, string st, string dt, int id)
        {
            clientFullname = fullname;
            state = st;
            dateTime = dt;
            this.id = id;
        }
    }
    public class ServicesList
    {
        public string serviceName { get; private set; }
        public string price { get; private set; }
        public ServicesList(string sn, string pr)
        {
            serviceName = sn;
            price = pr;
        }
    }
}
