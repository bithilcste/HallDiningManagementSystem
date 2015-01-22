using System.ComponentModel;
using System.Windows;

namespace WpfApplication4
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MealEntryItem_OnClick(object sender, RoutedEventArgs e)
        {
           Panel.Children.Clear();
            MealUI meal = new MealUI();
            Panel.Children.Add(meal);
        }

        private void FeastItem_OnClick(object sender, RoutedEventArgs e)
        {
          //  this.NavigationService.Navigate(new Uri("FeastUI.xaml",UriKind.Relative));
            Panel.Children.Clear();
            FeastUI mUI=new FeastUI();
            Panel.Children.Add(mUI);
        }

        private void CalculationItem_OnClick(object sender, RoutedEventArgs e)
        {
            Panel.Children.Clear();
            var mCalculationUi = new CalculationUI();
            Panel.Children.Add(mCalculationUi);
        }

        private void BlackListItem_OnClick(object sender, RoutedEventArgs e)
        {
            Panel.Children.Clear();
            var mBlaclkListUI = new BlackListUI();
            Panel.Children.Add(mBlaclkListUI);
        }

        private void AboutDmsItem_OnClick(object sender, RoutedEventArgs e)
        {
            Panel.Children.Clear();
            var abouUi = new AboutUi();          
            Panel.Children.Add(abouUi);
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            MessageBoxResult key = MessageBox.Show("Are you sure you want to quit?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            e.Cancel = (key == MessageBoxResult.No);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Panel.Children.Clear();
            StudentUI student=new StudentUI();        
            Panel.Children.Add(student);
        }

        private void homeDmsItem_OnClick(object sender, RoutedEventArgs e)
        {
            var main = new LoginWindow();
            Close();
            main.ShowDialog();

        }       
    }
}