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

namespace Reolmarkedet { 

    public partial class CreateRenter : Page
    {
        public CreateRenter()
        {
            InitializeComponent();
        }

        string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

        private void Clear()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtStreet.Clear();
            txtHouseNumber.Clear();
            txtPostalCode.Clear();
            txtCity.Clear();
            txtPhoneNumber.Clear();
            txtEmail.Clear();
            txtAccountInfo.Clear();            
        }
        private void BackButton(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void Create_Renter(object sender, RoutedEventArgs e)
        {
            string error = "";
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                // Get a list of all available stands from the database.
                SqlCommand command = new SqlCommand("SELECT StandID FROM STANDS WHERE Available = 1", connection);
                SqlDataReader reader = command.ExecuteReader();
                List<int> availableStandIDs = new List<int>();
                while (reader.Read())
                {
                    availableStandIDs.Add(reader.GetInt32(0));
                }
                reader.Close();

                // Select a random available stand from the list.
                int randomStandID = availableStandIDs[new Random().Next(availableStandIDs.Count)];

                // Set the stand_id parameter in the INSERT INTO RENTERS statement to the selected stand ID.
                command = new SqlCommand("INSERT INTO RENTERS (FirstName, LastName, Address, HouseNumber, Zip, City, Phonenumber, Emailaddress, BankaccountDetails, StandID) VALUES (@FirstName, @LastName, @Address, @HouseNumber, @Zip, @City, @Phonenumber, @Emailaddress, @BankaccountDetails, @StandID)", connection);
                command.Parameters.Add(CreateParam("@FirstName", txtFirstName.Text.Trim(), SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@LastName", txtLastName.Text.Trim(), SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@Address", txtStreet.Text.Trim(), SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@HouseNumber", txtHouseNumber.Text.Trim(), SqlDbType.Int));
                command.Parameters.Add(CreateParam("@Zip", txtPostalCode.Text.Trim(), SqlDbType.Int));
                command.Parameters.Add(CreateParam("@City", txtCity.Text.Trim(), SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@PhoneNumber", txtPhoneNumber.Text.Trim(), SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@Emailaddress", txtEmail.Text.Trim(), SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@BankaccountDetails", txtAccountInfo.Text.Trim(), SqlDbType.NVarChar));
                command.Parameters.Add(CreateParam("@StandID", randomStandID, SqlDbType.Int));

                // Insert renter in the database and set stand available to false.
                command.ExecuteNonQuery();
                MessageBox.Show("Lejer er nu oprettet!\nDu er tildelt stand nummer " + randomStandID);
                Clear();
                command = new SqlCommand("UPDATE STANDS SET Available = 0 WHERE StandID = @StandID", connection);
                command.Parameters.Add(CreateParam("@StandID", randomStandID, SqlDbType.Int));
                command.ExecuteNonQuery();
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
    }
}
