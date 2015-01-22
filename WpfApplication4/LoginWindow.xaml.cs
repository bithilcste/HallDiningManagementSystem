using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;

namespace WpfApplication4
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {

        public static String ConnectionString = @"server=BITHIL-PC;Database=Dinning;Integrated Security=true";

        public static SqlConnection Conn;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Btnlogin_Click(object sender, RoutedEventArgs e)
        {

            if (Txtname.Text == string.Empty || PasswdBox.Password == string.Empty)
            {
                MessageBox.Show("Both field are Required!!!", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (!Regex.Match(Txtname.Text, @"^([A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Error in User Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (!Regex.Match(PasswdBox.Password, @"^([a-z0-9_-]{6,18})$").Success)
            {
                MessageBox.Show("Error in  Password", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (Usertype.Text == string.Empty)
            {
                MessageBox.Show("Please Select user type!!!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            else if (Usertype.Text == "Manager")
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("select name,passwd from  manager  where name=@name and passwd=@pass", connection);
                    cmd.Parameters.AddWithValue("@name", Txtname.Text);
                    cmd.Parameters.AddWithValue("@pass", PasswdBox.Password);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = 0;
                    while (reader.Read())
                    {
                        count++;
                    }
                    if (count == 1)
                    {
                        var main = new MainWindow();
                        Close();
                        main.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Username or Password  or Usertype is Wrong try again", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                }
            }
            else if (Usertype.Text == "Admin")
            {
              
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("select Name,password from  admin  where Name=@name and Password=@pass", connection);
                    cmd.Parameters.AddWithValue("@name", Txtname.Text);
                    cmd.Parameters.AddWithValue("@pass", PasswdBox.Password);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = 0;
                    while (reader.Read())
                    {
                        count++;
                    }
                    if (count == 1)
                    {
                        ManagerUI manager = new ManagerUI();
                        Close();
                        manager.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Username or Password or Usertype is Wrong try again", "MessageBox", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            PasswdBox.Password = string.Empty;
        }
    }
}