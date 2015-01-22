using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication4
{
    /// <summary>
    ///     Interaction logic for CalculationUI.xaml
    /// </summary>
    public partial class CalculationUI
    {
        String ConnectionString = @"server=BITHIL-PC;Database=Dinning;Integrated Security=true";

        public CalculationUI()
        {
            InitializeComponent();
        }

        private void Btnsave_Click(object sender, RoutedEventArgs e)
        {
            if (Combomanagerid.SelectedItem == null)
            {
                MessageBox.Show("Select a manager Id!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else if (Txtamount.Text == string.Empty)
            {
                MessageBox.Show("Enter amount", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                bool value = Regex.IsMatch(Txtamount.Text, @"^([0]||[1-9][0-9]||[1-9][0-9][0-9]||[1-9][0-9][0-9][0-9]||[1-9][0-9][0-9][0-9][0-9])$");

                if (value)
                {
                    String query = string.Format("insert into market values('{0}','{1}','{2}','{3}')",
                        Combomanagerid.SelectionBoxItem, Combomanagername.SelectionBoxItem, DtPicker.Text,
                        Txtamount.Text);
                    var connection = new SqlConnection(ConnectionString);

                    try
                    {
                        connection.Open();
                        var cmd = new SqlCommand(query, connection);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data is inserted Successfully!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Error in Amount Format", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            } 
        }

        private void Combomanagerid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Marketingdate,Amount from market where Id=@id", connection);
                cmd.Parameters.AddWithValue("@id", CombomanagerId1.SelectedItem);
                var ds = new DataSet();
                var adapter = new SqlDataAdapter();

                connection.Open();
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
                Listmanager.DataContext = ds.Tables[0].DefaultView; // ListView

                if (ds.Tables[0].DefaultView.Count == 0)
                {
                    MessageBox.Show("Not found!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }

        private void Btnupdate_Click(object sender, RoutedEventArgs e)
        {
            bool value = Regex.IsMatch(Txtamount1.Text, @"^([0]||[1-9][0-9]||[1-9][0-9][0-9]||[1-9][0-9][0-9][0-9]||[1-9][0-9][0-9][0-9][0-9])$");

            if (value)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update  market  set Amount=@d1  where Id=@d2", connection);

                    cmd.Parameters.AddWithValue("@d1", Txtamount1.Text);
                    cmd.Parameters.AddWithValue("@d2", CombomanagerId2.SelectionBoxItem);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Your Data is Updated Successfully!!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            else
            {
                MessageBox.Show("Marketing Expenses must be greater than 9 and lessthan 100000", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void Btncalculate_Click(object sender, RoutedEventArgs e)
        {
            Txttotalmeal.Text = TotalMeal().ToString(CultureInfo.InvariantCulture);
            Txtmarketingexpenses.Text = TotalMarketExpense().ToString(CultureInfo.InvariantCulture);
            Txtmealrate.Text = (TotalMarketExpense() / TotalMeal()).ToString(CultureInfo.InvariantCulture);
        }

        private int TotalMarketExpense()
        {
            int market = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select sum(Amount) from market", connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    market = int.Parse(reader[0].ToString());
                }
            }
            return market;
        }

        private int TotalMeal()
        {
            int meal = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select sum(Noofmeal) from meal", connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    meal = int.Parse(reader[0].ToString());
                }
            }
            return meal;
        }
        
        private void Btncheck_Click(object sender, RoutedEventArgs e)
        {
            if (Txtcheckid.Text == string.Empty)
            {
                MessageBox.Show("Enetr Id first to check!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else
            {
                bool value = Regex.IsMatch(Txtcheckid.Text, @"^([0-9]||[1-9][0-9]||[1-9][0-9][0-9]||[1-9][0-9][0-9][0-9])$");

                if (value)
                {
                    if (Txtcheckid.Text == Checkid().ToString(CultureInfo.InvariantCulture))
                    {


                        int totalmoney = InitalPayment();
                        int money = IndivisulsalTotalMeal() * (TotalMarketExpense() / TotalMeal()) - totalmoney;

                        if (money > 0)
                        {
                            MessageBox.Show(" Mr." + Name1() + ",ur total meal is " + IndivisulsalTotalMeal() +
                                            " and u have paid tk." + totalmoney + ",so now u have pay tk." + money +
                                            " more to the manager.");
                        }
                        else
                        {
                            money = money * (-1);
                            MessageBox.Show(" Mr." + Name1() + ",Your total Meal is " + IndivisulsalTotalMeal() +
                                            " and You have Paid tk. " + totalmoney + ",So Now You will get back tk." + money +
                                            " From the Manager.", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                    }


                    else
                    {
                        MessageBox.Show("Invalid Id Number", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                }

                else
                {
                    MessageBox.Show("Error in Id Format", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            GetIdname();
            DtPicker.Text = DateTime.Today.ToString(CultureInfo.InvariantCulture);
        }
        private string Name1()
        {
            string name = "";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Name from boarder where Id=@id", connection);

                cmd.Parameters.AddWithValue("@id", Txtcheckid.Text);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name = reader[0].ToString();
                }
            }
            return name;
        }

        private int IndivisulsalTotalMeal()
        {
            int meal = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select sum(Noofmeal) from meal where Id=@id", connection);

                cmd.Parameters.AddWithValue("@id", Txtcheckid.Text);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    meal = int.Parse(reader[0].ToString());
                }
            }
            return meal;
        }

        private int InitalPayment()
        {
            int initialpayment = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select InitialPayment from boarder where Id=@id", connection);

                cmd.Parameters.AddWithValue("@id", Txtcheckid.Text);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    initialpayment = int.Parse(reader[0].ToString());
                }
            }
            return initialpayment;
        }

        private void GetIdname()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Id,name from manager", connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string sid = reader[0].ToString();
                    string sname = reader[1].ToString();
                    Combomanagerid.Items.Add(sid);
                    CombomanagerId1.Items.Add(sid);
                    Combomanagername.Items.Add(sname);
                    CombomanagerId2.Items.Add(sid);
                }
            }
        }

        private void Combomanagerid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Name from manager where Id=@id", connection);
                cmd.Parameters.AddWithValue("@id", Combomanagerid.SelectedItem);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string sname = reader[0].ToString();
                    Combomanagername.Text = sname;
                }
            }
        }

        private void CombomanagerId2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Marketingdate,Amount from market where Id=@id", connection);
                cmd.Parameters.AddWithValue("@id", CombomanagerId2.SelectedItem);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string smarketingdate = reader[0].ToString();
                        string samount = reader[1].ToString();
                        DtPicker1.Text = smarketingdate;
                        Txtamount1.Text = samount;
                    }
                }
                else
                {
                    DtPicker1.Text = string.Empty;
                    Txtamount1.Text = string.Empty;
                    MessageBox.Show("Not found");
                }
            }
        }

        private int Checkid()
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Id from boarder where Id=@id", connection);
                cmd.Parameters.AddWithValue("@id", Txtcheckid.Text);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    id = int.Parse(reader[0].ToString());
                }
            }
            return id;
        }    

        private void TC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var ti = TC.SelectedItem as TabItem;
            //if (ti != null) Title = ti.Header.ToString();
        }     
    }
}