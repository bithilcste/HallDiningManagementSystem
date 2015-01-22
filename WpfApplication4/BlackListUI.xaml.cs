using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication4
{
    /// <summary>
    ///     Interaction logic for BlackListUI.xaml
    /// </summary>
    public partial class BlackListUI
    {
        String ConnectionString = @"server=BITHIL-PC;Database=Dinning;Integrated Security=true";

        public BlackListUI()
        {
            InitializeComponent();
        }

        private void TC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        //    var ti = TC.SelectedItem as TabItem;
        //    if (ti != null) Title = ti.Header.ToString();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (MinimumamountTextBox.Text.Length == 0)
            {
                MessageBox.Show("Please Enter Minimum Amount to start meal", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {

                bool value = Regex.IsMatch(MinimumamountTextBox.Text, @"^([0]||[5-9][0-9]||[1-9][0-9][0-9])$");

                if (value)
                {
                    String query = string.Format("insert into minamount values('{0}')", MinimumamountTextBox.Text);
                    SqlConnection connection = new SqlConnection(ConnectionString);
                    try
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data is inserted Succesfully!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Error in Minimum Amount Format", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }

        private void SubmitButton1_Click(object sender, RoutedEventArgs e)
        {
            if (MinimumTextBox.Text.Length == 0)
            {
                MessageBox.Show("Enter Minimum Amount Know the List!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                bool value = Regex.IsMatch(MinimumTextBox.Text, @"([0]||[5-9][0-9]|[1-9][0-9][0-9])$");
                if (value)
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand("select Id,Name,Deptname,Initialpayment from boarder where (InitialPayment< @d1)", connection);
                        cmd.Parameters.AddWithValue("@d1", MinimumTextBox.Text);
                        DataSet ds = new DataSet();
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        connection.Open();
                        adapter.SelectCommand = cmd;
                        adapter.Fill(ds);
                        BorderListView.DataContext = ds.Tables[0].DefaultView; // ListView
                        MessageBox.Show("This is the list of boarder whose payment is equal or less than " +LowestAmountPayed());
                    }
                }
                else
                {
                    MessageBox.Show("Error in Minimum Amount Format", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }
        private int LowestAmountPayed()
        {
            int slowestamount = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
            SqlCommand cmd = new SqlCommand("select Minamount from minamount",connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    slowestamount = int.Parse(reader[0].ToString());
                }
            }
            return slowestamount;
        }

    }
}