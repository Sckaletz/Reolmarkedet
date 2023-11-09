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
using IronBarCode;

namespace Reolmarkedet
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Page
    {
        public AddProduct()
        {
            InitializeComponent();
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        private void Clear()
        {
            txtStandID.Clear();

            txtDescription.Clear();
            txtPrice.Clear();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            string error = "";
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                // Generate a random 10 digit number.
                int randomNumber = GenerateRandom10DigitNumber();

                // Set the stand_id parameter in the INSERT INTO RENTERS statement to the selected stand ID.
                SqlCommand command = new SqlCommand("INSERT INTO PRODUCTS (StandId, Price, Description, Barcode) VALUES (@StandId, @Price, @Description, @Barcode)", connection);
                command.Parameters.Add(CreateParam("@StandId", txtStandID.Text.Trim(), SqlDbType.Int));
                command.Parameters.Add(CreateParam("@Price", txtPrice.Text.Trim(), SqlDbType.Float));
                command.Parameters.Add(CreateParam("@Description", txtDescription.Text.Trim(), SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@Barcode", randomNumber, SqlDbType.Int));
                command.ExecuteNonQuery();
                Clear();
                MessageBox.Show("Produkt oprettet!");
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

        private SqlParameter CreateParam(string name, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(name, type);
            param.Value = value;
            return param;
        }

        private int GenerateRandom10DigitNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(100000000, 999999999);
            return randomNumber;
        }
    }
}
