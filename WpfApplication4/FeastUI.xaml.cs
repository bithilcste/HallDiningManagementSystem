using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for FeastUI.xaml
    /// </summary>
    public partial class FeastUI
    {
        String ConnectionString = @"server=BITHIL-PC;Database=Dinning;Integrated Security=true";

        public FeastUI()
        {
            InitializeComponent();

        }

        private void FeastEntryButton_Click(object sender, RoutedEventArgs e)
        {
            if (Feastrate.Text.Length == 0)
            {
                MessageBox.Show("Enter feast rate first!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else if (!Regex.Match(Feastrate.Text, @"^([5-9][0-9]|1[0-9][0-9])$").Success)
            {
                MessageBox.Show("Feasrate must be greater than or equal to 50 and less than 100", "Message",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                String query = string.Format("insert into feastrate values('{0}')", Feastrate.Text);
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data is inserted Successfully!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            //GetData();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            int tfeast;

            if (!Regex.Match(IdTextBox.Text, @"^([1-9]|[1-9][0-9]|[1-9][0-9][0-9]}[1-9][0-9][0-9][0-9])$").Success)
            {
                MessageBox.Show("Error in ID", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!Regex.Match(NooffeastTextBox.Text, @"[1-9]|[1-9][0-9]").Success)
            {
                MessageBox.Show("Error in feast entry!!", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (!Regex.Match(AmountTextBox.Text, @"^([0]||[5-9][0-9]||1[0-9][0-9])$").Success)
            {
                MessageBox.Show("Error in feast amount entry!!", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if ((IdTextBox.Text.Length == 0) && (NooffeastTextBox.Text.Length == 0) &&
                     (AmountTextBox.Text.Length == 0))
            {
                MessageBox.Show("Enter Id ,NoofFeast and  Amount !!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                int id = Convert.ToInt32(IdTextBox.Text);
                int nooffeast = Convert.ToInt32(NooffeastTextBox.Text);
                int amount = Convert.ToInt32(AmountTextBox.Text);
                int feastrate = Convert.ToInt32(FeastrateText.Text);

                if (id == Convert.ToInt32(CheckId()))
                {
                    MessageBox.Show("This id is already used by other person.", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }

                else
                {

                    if (amount <= feastrate * nooffeast || amount > feastrate * nooffeast)
                    {
                        int amount1 = amount;
                        int no = amount / feastrate;
                        int returntk = amount1 - feastrate * no;
                        int feastamount = feastrate * no;

                        if (returntk < 0)
                        {
                            returntk = -1 * returntk;
                        }

                        int noOfFeast;
                        if (no > 0)
                        {
                            noOfFeast = no;
                            tfeast = no + Getfeast();
                        }
                        else
                        {
                            noOfFeast = 0;
                            tfeast = 0 + Getfeast();
                        }

                        String query = string.Format("insert into feastentry values('{0}','{1}','{2}','{3}')", id, FeastDatePicker.Text, noOfFeast,
                            feastamount);

                        string query1 =
                            string.Format("update boarder set totalfeast='" + tfeast + "' where Id='" + IdTextBox.Text + "'");

                        SqlConnection connection = new SqlConnection(ConnectionString);
                        try
                        {
                            connection.Open();
                            SqlCommand cmd = new SqlCommand(query, connection);
                            SqlCommand cmd1 = new SqlCommand(query1, connection);
                            cmd.ExecuteNonQuery();
                            cmd1.ExecuteNonQuery();
                            MessageBox.Show("You will get " + no + " feast and tk. " + returntk + " back and No of Feast is updated", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            Reset();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    ClearBoxes();
                }
            }
        }

        private void ClearBoxes()
        {
            IdTextBox.Text = string.Empty;
            NooffeastTextBox.Text = string.Empty;
            AmountTextBox.Text = string.Empty;
            CheckIdTextBox.Text = string.Empty;
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Id,Nooffeast from feastentry where Id=@id", connection);
                cmd.Parameters.AddWithValue("@id", CheckIdTextBox.Text);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int sid = int.Parse(reader[0].ToString());
                        int snumber = int.Parse(reader[1].ToString());

                        MessageBox.Show("Your Registration is done your id is " + sid + " and You will get " + snumber +
                                        " feast.", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                }
                else
                {
                    MessageBox.Show("Your id is not registered for feast!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                ClearBoxes();
            }  
        }
        private void GetData()
        {
            string id;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Amount from feastrate ", connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id = reader[0].ToString();
                    FeastrateText.Text = id;
                }
            }
        }



        private string CheckId()
        {
            string id = null;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Id from feastentry", connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id = reader[0].ToString();
                }

            }
            return id;
        }
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var ti = TC.SelectedItem as TabItem;
            //if (ti != null) Title = ti.Header.ToString();
        }

        private int Getfeast()
        {
            int sfeast = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select totalfeast from boarder where Id=@id", connection);
                cmd.Parameters.AddWithValue("@id", IdTextBox.Text);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sfeast = int.Parse(reader[0].ToString());
                }
            }
            return sfeast;
        }
        private void Reset()
        {
            NooffeastTextBox.Text = string.Empty;
            AmountTextBox.Text = string.Empty;
            IdTextBox.Text = string.Empty;
        }

        private void TabItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            FeastDatePicker.Text = DateTime.Today.ToString(CultureInfo.InvariantCulture);
        }
    }
}



