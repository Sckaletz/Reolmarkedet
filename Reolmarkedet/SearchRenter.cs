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
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.ConstrainedExecution;

namespace Reolmarkedet
{

    public partial class SearchRenter : Page
    {
        public SearchRenter()
        {
            InitializeComponent();
        }

        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

        private void Clear()
        {
            txtemailbox.Clear();
        }
        private void BackButton(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Search_Renter(object sender, RoutedEventArgs e)
        {
            string input = txtemailbox.Text;

            string error = "";
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM RENTERS WHERE Emailaddress = @searched", connection);
                command.Parameters.AddWithValue("@searched", input);


                DataTable dataTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    string result = "";
                    foreach (DataRow row in dataTable.Rows)
                    {
                        result += row["FirstName"] + "\n" + row["LastName"] + "\n" + row["Address"] + " " + row["HouseNumber"] + "\n" + row["RenterId"];
                    }
                    Clear();
                    MessageBox.Show(result);
                    return;
                }

                else
                {
                    Clear();
                    MessageBox.Show("Ingen lejere fundet.");
                }
                connection.Close();
                return;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            MessageBox.Show(error);
        }
    }
}
