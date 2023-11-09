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
//using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.ConstrainedExecution;

namespace Reolmarkedet
{
    /// <summary>
    /// Interaction logic for OpretReol.xaml
    /// </summary>
    public partial class CreateStand : Page
    {
        public CreateStand()
        {
            InitializeComponent();
        }
        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        private void Clear()
        {
            txtType.Clear();

        }
        private void BackButton(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            string error = "";
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);


                SqlCommand command = new SqlCommand("INSERT INTO STANDS (Type, Available) VALUES (@type, @available)", connection);
                command.Parameters.Add(CreateParam("@type", txtType.Text.Trim(), SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@available", true, SqlDbType.Bit));
                connection.Open();
                MessageBox.Show("Reol oprettet!");

                if (command.ExecuteNonQuery() == 1)
                {
                    Clear();
                    return;
                }
                error = "Illegal database operation";
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

        private SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }
    }
}
