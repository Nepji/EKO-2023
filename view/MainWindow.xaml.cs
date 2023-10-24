using System;
using System.Collections.Generic;
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

namespace EKO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new viewmodel.MainViewModel();
        }

        private void EnterPriseRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            model.DataBase.getInstance()._currentTable = model.TablesNames.enterprise;
        }

        private void PollutionRadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            model.DataBase.getInstance()._currentTable = model.TablesNames.pollution;
        }

        private void PollutantRadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            model.DataBase.getInstance()._currentTable = model.TablesNames.pollutant;
        }
    }
}
