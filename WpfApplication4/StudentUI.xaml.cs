using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace WpfApplication4
{
    /// <summary>
    ///     Interaction logic for StudentUI.xaml
    /// </summary>
    public partial class StudentUI
    {
        String ConnectionString = @"server=BITHIL-PC;Database=Dinning;Integrated Security=true";

        DataSet ds;
        string imageName;

        public StudentUI()
        {
            InitializeComponent();
        }

        private void Btnupdate_Click(object sender, RoutedEventArgs e)
        {
            if (!Regex.Match(Txtboardername1.Text,@"^([A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Error Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!Regex.Match(TxtbordermobileNo1.Text, @"^(\d{10}||\d{11})$").Success)
            {
                MessageBox.Show("Error Mobile Number", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (!Regex.Match(Txtborderadress1.Text, @"^([A-Z][a-z]+||\d{3}-[A-z][a-z]+\s[A-Z][a-z]+,[A-Z][a-z]+,[A-Z][a-z]+)$").Success)
            {

                MessageBox.Show("Given the Address Field ", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (!Regex.Match(TxtInitialPayment1.Text, @"^([0]|[5-9][0-9]|[1-9][0-9][0-9])$").Success)
            {

                MessageBox.Show("Payment Amount should be grater than 49 and less than 1000", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (true)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update boarder set Name=@d1, Deptname=@d2, Batch=@d3, MobileNo=@d4,Address1=@d5, Initialpayment=@d6 where Id=@d7",connection);
                    cmd.Parameters.AddWithValue("@d1",Txtboardername1.Text);
                    cmd.Parameters.AddWithValue("@d2", Combodeptname1.SelectionBoxItem);
                    cmd.Parameters.AddWithValue("@d3",Txtboarderbatch1.SelectionBoxItem );
                    cmd.Parameters.AddWithValue("@d4",TxtbordermobileNo1.Text );
                    cmd.Parameters.AddWithValue("@d5",Txtborderadress1.Text);
                    cmd.Parameters.AddWithValue("@d6",TxtInitialPayment1.Text );
                    cmd.Parameters.AddWithValue("@d7",TxtborderId1.SelectionBoxItem );
        
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Your Data is Updated Successfully!!!","Message",MessageBoxButton.OK,MessageBoxImage.Asterisk);
                }
            }
        }
        private void Btnsave_Click(object sender, RoutedEventArgs e)
        {

            if (Txtboardername.Text == string.Empty)
            {
                MessageBox.Show("Enter Border Name!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else if (Combodeptname.SelectionBoxItem == "")
            {
                MessageBox.Show("Select Border Deptarment Name!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else if (Txtborderbatch.SelectionBoxItem == "")
            {
                MessageBox.Show("Select Border  batch!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else if (TxtbordermobileNo.Text == string.Empty)
            {
                MessageBox.Show("Enter Border Mobile Number!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else if (Txtborderadress.Text == string.Empty)
            {
                MessageBox.Show("Enter Border Address!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            else if (TxtInitialPayment.Text == string.Empty)
            {
                MessageBox.Show("Enter Initial Payment!!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else if (image1.ToString() == "")
            {
                MessageBox.Show("Please Upload an Image !!!","Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                int id = Convert.ToInt32(TxtborderId.Text);
                if (id == Convert.ToInt32(CheckId()))
                {
                    MessageBox.Show("This id is already used by other person.", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else if (!Regex.Match(Txtboardername.Text,
                                 @"^([A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)
                {
                    MessageBox.Show("Error in Name Format", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else if (!Regex.Match(TxtbordermobileNo.Text, @"^(\d{10}||\d{11})$").Success)
                {

                    MessageBox.Show("Error in Mobile Number", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else if (
                    !Regex.Match(Txtborderadress.Text,
                                 @"^([A-Z][a-z]+||\d{3}-[A-z][a-z]+\s[A-Z][a-z]+,[A-Z][a-z]+,[A-Z][a-z]+)$").Success)
                {
                    MessageBox.Show("Error in Address Field", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (!Regex.Match(TxtInitialPayment.Text, @"^(0||[5-9][0-9]||[1-9][0-9][0-9])$").Success)
                {
                    MessageBox.Show(" Payment Amount should be grater than 49 and less than 1000", "Message",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (true)
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        //Initialize a file stream to read the image file
                        FileStream fs = new FileStream(@imageName, FileMode.Open, FileAccess.Read);
                        //Initialize a byte array with size of stream
                        byte[] imgByteArr = new byte[fs.Length];
                        //Read data from the file stream and put into the byte array
                        fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
                        fs.Close();

                        int tfeast = 0;
                        int tmeal = 0;
                        conn.Open();
                        string sql =
                            "insert into boarder(Id,Name,Deptname,Batch,Mobileno,Address1,Initialpayment,totalmeal,totalfeast,Image) values(@d0,@d1,@d2,@d3,@d4,@d5,@d6,@d7,@d8, @img)";

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@d0", TxtborderId.Text);
                            cmd.Parameters.AddWithValue("@d1", Txtboardername.Text);
                            cmd.Parameters.AddWithValue("@d2", Combodeptname.SelectionBoxItem);
                            cmd.Parameters.AddWithValue("@d3", Txtborderbatch.Text);
                            cmd.Parameters.AddWithValue("@d4", TxtbordermobileNo.Text);
                            cmd.Parameters.AddWithValue("@d5", Txtborderadress.Text);
                            cmd.Parameters.AddWithValue("@d6", TxtInitialPayment.Text);
                            cmd.Parameters.AddWithValue("@d7", tmeal);
                            cmd.Parameters.AddWithValue("@d8", tfeast);
                            cmd.Parameters.Add(new SqlParameter("@img", imgByteArr));

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Data is inserted Successfully!!", "Message", MessageBoxButton.OK,
                                            MessageBoxImage.Asterisk);
                                            
                        }
                    }
                }
            }
        }
        private string CheckId()
        {
            string id = null;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Id from boarder", connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id = reader[0].ToString();
                }

            }
            return id;
        }

        private void GetId()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                 SqlCommand cmd = new SqlCommand("select Id from boarder",connection);

                connection.Open();           
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int sid = int.Parse(reader[0].ToString());
                    TxtborderId1.Items.Add(sid);

                    int id = sid + 1;
                    TxtborderId.Text = id.ToString(CultureInfo.InvariantCulture);
                }
            }
        }


        private void TabItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            GetId();         
        }

        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
               var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private void TxtborderId1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
             SqlCommand cmd = new SqlCommand("select Name,Deptname,Batch,MobileNo,Address1,Initialpayment,Image from boarder where Id=@id",connection);
                
                cmd.Parameters.AddWithValue("@id", TxtborderId1.SelectedItem); 
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string sname = reader[0].ToString();
                    string sdeptname = reader[1].ToString();
                    string sbatch = reader[2].ToString();
                    string smobileno = reader[3].ToString();
                    string sadress = reader[4].ToString();
                    string sinitialpayment = reader[5].ToString();
                    MemoryStream ms = new MemoryStream((byte[])reader[6]);
                    image2.Source = ToImage(ms.ToArray());

                    Txtboardername1.Text = sname;
                    Txtboarderbatch1.Text = sbatch;
                    Combodeptname1.Text = sdeptname;
                    Txtborderadress1.Text = sadress;
                    TxtbordermobileNo1.Text = smobileno;
                    TxtInitialPayment1.Text = sinitialpayment;
                }
            }
        }

        //private void TabControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    var tab = TC.SelectedItem as TabItem;
        //    if (tab != null) Title = tab.Header.ToString();
        //}
        private void GetBatch()
        {           
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
             SqlCommand cmd = new SqlCommand("select Batchname from batch",connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string sname = reader[0].ToString();
                Txtborderbatch.Items.Add(sname);
                Txtboarderbatch1.Items.Add(sname);

            }
          }
        }

        private void GetDepartment()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
             SqlCommand cmd = new SqlCommand("select Departmentname from department",connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string sname = reader[0].ToString();
                    Combodeptname.Items.Add(sname);
                    Combodeptname1.Items.Add(sname);
                }
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            GetBatch();
            GetDepartment();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fldlg.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
                fldlg.ShowDialog();
                {

                    imageName = fldlg.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    image1.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));

                }

                fldlg = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        
    }
}