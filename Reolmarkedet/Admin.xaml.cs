using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;
//using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.ConstrainedExecution;

namespace Reolmarkedet
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        public Admin()
        {
            InitializeComponent();
        }
        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

        private void Create_Bookcase(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateStand());
        }

        private void Create_Renter(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateRenter());
        }

        private void Search_Renter(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SearchRenter());
        }

        private void Add_Product(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProduct());
        }

        private void Sale(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Sale());
        }
        private void BackButton(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Initialize(object sender, RoutedEventArgs e)
        {
            //string error = "";
            //SqlConnection connection = null;

            string tableNameStands = "STANDS";
            string tableNameRenters = "RENTERS";
            string tableNameProducts = "PRODUCTS";
            string tableNameSales = "SALES";

            // SQL statement to create the Stands table
            string createTableSqlStands = @"
            CREATE TABLE " + tableNameStands + @" (
                StandId INT PRIMARY KEY IDENTITY(1,1),
                Available BIT,
                Type NVARCHAR(255)
            )";

            // SQL statement to create the Renters table
            string createTableSqlRenters = @"
            CREATE TABLE " + tableNameRenters + @" (
                RenterId INT PRIMARY KEY IDENTITY(1,1),
                FirstName NVARCHAR(255) NOT NULL,
                LastName NVARCHAR(255) NOT NULL,
                Address NVARCHAR(255) NOT NULL,
                HouseNumber INT NOT NULL,
                Zip INT NOT NULL,
                City NVARCHAR(255) NOT NULL,
                Phonenumber NVARCHAR(255) NOT NULL,
                Emailaddress NVARCHAR(255) NOT NULL,
                BankaccountDetails NVARCHAR(255) NOT NULL,
                StandId INT,
                FOREIGN KEY (StandId) REFERENCES " + tableNameStands + @"(StandId)
            )";

            // SQL statement to create the Products table
            string createTableSqlProducts = @"
            CREATE TABLE " + tableNameProducts + @" (
                ItemId INT PRIMARY KEY IDENTITY(1,1),
                Description NVARCHAR(255),
                Price FLOAT NOT NULL,
                Barcode INT NOT NULL,
                RenterId INT,
                StandId INT,
                FOREIGN KEY (RenterId) REFERENCES " + tableNameRenters + @"(RenterId),
                FOREIGN KEY (StandId) REFERENCES " + tableNameStands + @"(StandId)
            )";

            // SQL statement to create the Sales table
            string createTableSqlSales = @"
            CREATE TABLE " + tableNameSales + @" (
                SaleId INT PRIMARY KEY IDENTITY(1,1),
                Date DATE,
                Time TIME,
                Products NVARCHAR(255),
                Totalprice FLOAT,
                RenterId INT,
                FOREIGN KEY (RenterId) REFERENCES " + tableNameRenters + @"(RenterId)
            )";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(createTableSqlStands, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand command = new SqlCommand(createTableSqlRenters, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand command = new SqlCommand(createTableSqlProducts, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand command = new SqlCommand(createTableSqlSales, connection))
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Databasen er nu klar!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
                
        }

        private SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }
    }
}
