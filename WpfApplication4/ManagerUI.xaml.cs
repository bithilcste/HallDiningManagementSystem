using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication4
{
    /// <summary>
    ///     Interaction logic for ManagerUI.xaml
    /// </summary>
    public partial class ManagerUI
    {
        String ConnectionString = @"server=BITHIL-PC;Database=Dinning;Integrated Security=true";
        public ManagerUI()
        {
            InitializeComponent();
        }

        private void Btnmanagerave_OnClick(object sender, RoutedEventArgs e)
        {
            if (TxtmanagernameBox.Text == string.Empty)
            {
                MessageBox.Show("Enter manager name first","Message",MessageBoxButton.OK,MessageBoxImage.Asterisk);
            }

            else if (PasswdBox.Password == string.Empty)
            {
                MessageBox.Show("Enter Password first!!","Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else if (PasswdBox1.Password == string.Empty)
            {
                MessageBox.Show("Enter Password Again To confirm!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else
            {
                //@"^([A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$" 

                bool value1 = Regex.IsMatch(TxtmanagernameBox.Text,
                    @"^([A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$");
                bool value2 = Regex.IsMatch(PasswdBox1.Password, @"^[a-z0-9_-]{6,18}$");
                bool value3 = Regex.IsMatch(PasswdBox.Password, @"^[a-z0-9_-]{6,18}$");

                int id = int.Parse(Txtmanagerid.Text);

                if (value1 && value2 && value3)
                {
                    if (PasswdBox.Password == PasswdBox1.Password)
                    {

                        if (id<= 4)
                           {
                            
                            String query = string.Format("insert into manager values('{0}','{1}','{2}','{3}')",
                                id, TxtmanagernameBox.Text,
                                PasswdBox.Password, Dtstartmanagering);
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
                            MessageBox.Show("Manager Cann't be more than four", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Enter same Password to Proceed", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Password is mustbe the combination of digits and alphabet and at least 6 length !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }

        private void GetAdminId()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
             SqlCommand cmd = new SqlCommand("select Id from manager",connection);
  
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int sid = int.Parse(reader[0].ToString());
                    sid += 1;

                    Txtmanagerid.Text = sid.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void TabItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            Dtstartmanagering.Text = DateTime.Today.ToString(CultureInfo.InvariantCulture);
            GetId();
            GetManager();
        }

        private void GetManager()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
            SqlCommand cmd = new SqlCommand("select Id,name,passwd from manager",connection);
            var ds = new DataSet();
            var adapter = new SqlDataAdapter();

                connection.Open();
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
                LstManager.DataContext = ds.Tables[0].DefaultView; // ListView
            }
        }

        private void BtnOkay_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnUpadate_Click(object sender, RoutedEventArgs e)
        {
            if (TxtConfirmpassBox.Password == string.Empty)
            {
                MessageBox.Show("Enter again Password to confirm it", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else
            {
                if (TxtpassBox.Text == TxtConfirmpassBox.Password)
                {
                  using (SqlConnection connection = new SqlConnection(ConnectionString))
                   {
                   SqlCommand cmd = new SqlCommand("update  manager  set name=@d1, passwd=@d2 where Id=@d3",connection);
                       cmd.Parameters.AddWithValue("@d1", Txtmanagername.Text);
                       cmd.Parameters.AddWithValue("@d2", TxtpassBox.Text);
                       cmd.Parameters.AddWithValue("@d3", ComboId.SelectedItem);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Your Data is Updated Successfully!!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        ClearValue();
                    }
                }

                else
                {
                    MessageBox.Show("Enter again Password to confirm it!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }

        private void ClearValue()
        {
            TxtConfirmpassBox.Password = string.Empty;
        }

        private void ComboId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
             SqlCommand cmd = new SqlCommand("select name,passwd from manager where Id=@d1",connection);

                cmd.Parameters.AddWithValue("@d1", ComboId.SelectedItem);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string sname = reader[0].ToString();
                    string spasswd = reader[1].ToString();

                    Txtmanagername.Text = sname;
                    TxtpassBox.Text = spasswd;
                }
            }
        }

        private void TabItem_Loaded_2(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void Load()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
            SqlCommand cmd = new SqlCommand("select Id from manager",connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string sid = reader[0].ToString();
                    ComboId.Items.Add(sid);
                }
            }
        }

        private void Btnokay_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
         using (SqlConnection connection = new SqlConnection(ConnectionString))
         {
             SqlCommand cmd = new SqlCommand("select name from manager where dateofstartingmanagering=@start" ,connection);
                cmd.Parameters.AddWithValue("@start", TxtDate);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string sname = reader[0].ToString();
                    ListManager.Items.Add(sname);
                    //MessageBox.Show(sname);
                }
            }
        }

        private void TC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ti = TC.SelectedItem as TabItem;
            if (ti != null) Title = ti.Header.ToString();
        }


        private void EditBatchButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (BatchTextBox.Text == string.Empty)
            {
                MessageBox.Show("Fill  the box!! ");
            }

            else if (!Regex.Match(BatchTextBox.Text, @"^(1st||2nd||3rd||[4-9]th||[12][0-9]th)$").Success)
            {
                MessageBox.Show(
                    "Error in Batch format.Batch should be written as firstnumber and then(st,nd,rdand th)", "Message",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (true)
            {
              using (SqlConnection connection = new SqlConnection(ConnectionString))
              {
                SqlCommand cmd = new SqlCommand("update  batch set Batchname=@d1 where Id=@d2" ,connection);
                  cmd.Parameters.AddWithValue("@d1", BatchTextBox.Text);
                  cmd.Parameters.AddWithValue("@d2", BatchIdComboBox.SelectionBoxItem);
  
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Your data is Updated!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }

        private void DepartmentButton_OnClick(object sender, RoutedEventArgs e)
        {

            if (DepartmentBox.Text == string.Empty)
            {
                MessageBox.Show("Fill all the boxes!! ", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else if (
                !Regex.Match(DepartmentBox.Text,
                    @"^([A-Z]+|[A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$")
                    .Success)
            {
                MessageBox.Show(
                    "Error in Department name format.Department Name should be written as (CSTE,Pharmacy and Computer Science and Telecommunication Engineering)",
                    "Message",MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (true)
            {
                String ConnectionString = @"server=BITHIL-PC;Database=Dinning;Integrated Security=true";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string sql = "insert into department(Id,Departmentname) values(@d1,@d2)";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@d1", DeptIdTextBox.Text);
                        cmd.Parameters.AddWithValue("@d2", DepartmentBox.Text);
                        cmd.ExecuteNonQuery();
             
                       MessageBox.Show("Data is inserted Successfully!!", "Message", MessageBoxButton.OK,
                                            MessageBoxImage.Asterisk);
                        connection.Close();
                    }
                }
            }
        }

        private void BatchButton_OnClick(object sender, RoutedEventArgs e)
        {

           if (BatchBox.Text == string.Empty)
            {
                MessageBox.Show("Fill all the boxes!! ", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else if (!Regex.Match(BatchBox.Text, @"^(1st||2nd||3rd||[4-9]th||[12][0-9]th)$").Success)
            {
                MessageBox.Show(
                    "Error in Batch format.Batch should be written as firstnumber and then(st,nd,rdand th)", "Message",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (true)
            {
                String query = string.Format("insert into batch values('{0}','{1}')", BatchIdBox.Text, BatchBox.Text);
                SqlConnection connection = new SqlConnection(ConnectionString);

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

        private void EditDepartmentButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DepartmentBox1.Text == string.Empty)
            {
                MessageBox.Show("Enter Department You want to update", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else if (
                !Regex.Match(DepartmentBox1.Text,
                    @"^([A-Z]+|[A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$")
                    .Success)
            {
                MessageBox.Show("Error In Department Format", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (true)
            {
               using (SqlConnection connection = new SqlConnection(ConnectionString))
               {
                SqlCommand cmd = new SqlCommand("update  department  set Departmentname=@d1 where Id=@d2",connection);
                   
                    cmd.Parameters.AddWithValue("@d1", DepartmentBox1.Text);
                    cmd.Parameters.AddWithValue("@d2", DeptIdCombobox.SelectionBoxItem);         
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Your Data is Updated Successfully!!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }

        private void AddAdminButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == string.Empty || AdminPasswordBox.Password == string.Empty ||
                PositionComboBox.Text == string.Empty)
            {
                MessageBox.Show("Enter all the fields and select position!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else if (
                !Regex.Match(NameTextBox.Text,
                    @"^([A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)

            {
                MessageBox.Show("Error In Admin Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else if (!Regex.Match(AdminPasswordBox.Password, @"^[a-z0-9_-]{6,18}$").Success)
            {
                MessageBox.Show("Error in Password ,Password length must be 6.", "Message", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            else if (PositionComboBox.Text == string.Empty)
            {
                MessageBox.Show("Select a Postion for the Admin", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else if (true)
            {
                if (GetCountAdminId() > 4)
                {
                    //string pass = FormsAuthentication.HashPasswordForStoringInConfigFile(AdminPasswordBox, "MD5");

                    String query = string.Format("insert into admin values('{0}','{1}','{2}','{3}')",
                        AdminIdTextBox.Text, NameTextBox.Text, AdminPasswordBox.Password,
                        PositionComboBox.SelectionBoxItem);
                    SqlConnection connection = new SqlConnection(ConnectionString);

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
                else
                {
                    MessageBox.Show("Admin Can't be more than four!!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }

        private void UpdateAdminButton_OnClick(object sender, RoutedEventArgs e)
        {
         if (!Regex.Match(NameTextBox1.Text, @"^([A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Error In Admin Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (!Regex.Match(AdminPasswordBox1.Password, @"^[a-z0-9_-]{6,18}$").Success)
            {
                MessageBox.Show("Error in Password ,Password length must be 6.", "Message", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            else if (!Regex.Match(NewAdminPasswordBox.Password, @"^[a-z0-9_-]{6,18}$").Success)
            {
                MessageBox.Show("Error in Password ,Password length must be 6.", "Message", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            else if (true)
            {
               using (SqlConnection connection = new SqlConnection(ConnectionString))
               {
                SqlCommand cmd = new SqlCommand("update admin set Name=@d1,Password=@d2,Position=@d3 where Id=@d4" ,connection);
                   cmd.Parameters.AddWithValue("@d1", NameTextBox1.Text);
                   cmd.Parameters.AddWithValue("@d2", NewAdminPasswordBox.Password);
                   cmd.Parameters.AddWithValue("@d3", PositionComboBox1.Text);
                   cmd.Parameters.AddWithValue("@d4", AdminIdComboBox.SelectionBoxItem);
                   connection.Open();
                   cmd.ExecuteNonQuery();
                   MessageBox.Show("Data is updated Successfully!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }

        private void OkayButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GetId()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
            SqlCommand cmd = new SqlCommand("select Id from Admin",connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int sid = int.Parse(reader[0].ToString());
                    AdminIdComboBox.Items.Add(sid);

                    int id = sid + 1;
                    AdminIdTextBox.Text = id.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void TabControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            GetAdminId();
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
            SqlCommand cmd = new SqlCommand("select * from Admin",connection);
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
   
                connection.Open();
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
                AdminListView.DataContext = ds.Tables[0].DefaultView; // ListView
            }
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            GetAdminId();
        }

        private void AdminIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           using (SqlConnection connection = new SqlConnection(ConnectionString))
           {
            SqlCommand cmd = new SqlCommand("select Name,Password,Position from admin where Id=@id ",connection);
               
               cmd.Parameters.AddWithValue("@id", AdminIdComboBox.SelectedItem);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string sname = reader[0].ToString();
                    string spassword = reader[1].ToString();
                    string sposition = reader[2].ToString();

                    NameTextBox1.Text = sname;
                    AdminPasswordBox1.Password = spassword;
                    PositionComboBox1.Text = sposition;
                }
            }
        }

        private int GetCountManagerId()
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
             SqlCommand cmd = new SqlCommand("select count(Id) from manager",connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i = int.Parse(reader[0].ToString());
                }

            }
            return i;
        }

        private int GetCountAdminId()
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
            SqlCommand cmd = new SqlCommand("select count(Id) from admin ",connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i = int.Parse(reader[0].ToString());
                }
            }
            return i;
        }



        private void ManagerUI_OnClosing(object sender, CancelEventArgs e)
        {
            MessageBoxResult key = MessageBox.Show("Are you sure you want to quit?", "Confirm", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No);
            e.Cancel = (key == MessageBoxResult.No);
        }

        private void GetBatchId()
        {
            int i;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
             SqlCommand cmd = new SqlCommand("select Id from batch",connection);
                connection.Open(); 
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i = int.Parse(reader[0].ToString());
                    BatchIdComboBox.Items.Add(i);
                    BatchIdBox.Text = (i + 1).ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void GetDepartmentId()
        {
            int i;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
            SqlCommand cmd = new SqlCommand("select Id from department",connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i = int.Parse(reader[0].ToString());
                    DeptIdCombobox.Items.Add(i);
                    DeptIdTextBox.Text = (i + 1).ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void TabItem_Loaded_3(object sender, RoutedEventArgs e)
        {
            GetBatchId();
        }

        private void TabItem_Loaded_4(object sender, RoutedEventArgs e)
        {
            GetDepartmentId();
        }

        private void DeptIdCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
            SqlCommand cmd = new SqlCommand("select Departmentname from department  where Id=@id",connection);
                cmd.Parameters.AddWithValue("@id", DeptIdCombobox.SelectedItem);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string sdept = reader[0].ToString();

                    DepartmentBox1.Text = sdept;
                }
            }
        }

        private void BatchIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
            SqlCommand cmd = new SqlCommand("select Batchname from batch  where Id=@id",connection );
                cmd.Parameters.AddWithValue("@id", BatchIdComboBox.SelectedItem);                      
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string sbatch = reader[0].ToString();

                    BatchTextBox.Text = sbatch;
                }
            }
        }
    }
}


