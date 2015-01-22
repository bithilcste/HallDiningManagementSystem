using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for Manager.xaml
    /// </summary>
    public partial class Manager : Window
    {
        public Manager()
        {
            InitializeComponent();
        }

        private void AddmemberBoxItem_OnSelected(object sender, RoutedEventArgs e)
        {
            Panel.Children.Clear();
            var page = new AddMemberPage();

            Panel.Children.Add(page);
        }

        private void UpdatememdberBoxItem_OnSelected(object sender, RoutedEventArgs e)
        {
            Panel.Children.Clear();
            var page = new AddMemberPage();

            Panel.Children.Add(page);
        }

        private void Member_List_OnSelected(object sender, RoutedEventArgs e)
        {

        }
    }
}
