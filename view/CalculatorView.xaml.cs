using MySql.Data.MySqlClient;
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

namespace EKO.view
{
    /// <summary>
    /// Interaction logic for CalculatorView.xaml
    /// </summary>
    public partial class CalculatorView : UserControl
    {
        private static bool chooseIMG = true;
        public CalculatorView()
        {
            InitializeComponent();
            //OUTDataGrid.Visibility = Visibility.Hidden;
            CR.IsReadOnly = true;
            CRrisk.IsReadOnly = true;
            HQ.IsReadOnly = true;
            HQRisk.IsReadOnly = true;

            PreLoad();
        }


        private void PreLoad()
        {
            string cmdtext = "select `Id`,`Name` from pollutant";
            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand(cmdtext, connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    pollutant.Items.Add($"{reader[0]}:{reader[1]}");
                }
            }
            command.CommandText = "select `Id`,`Name` from enterprise";
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    enterprise.Items.Add($"{reader[0]}:{reader[1]}");
                }
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            chooseIMG = !chooseIMG;
            BitmapImage bitmapImage = new BitmapImage();
            if (chooseIMG)
            {
                bitmapImage = new BitmapImage(new Uri("pack://application:,,,/EKO;component/model/CR.png"));    
            }
            else
            {
                bitmapImage = new BitmapImage(new Uri("pack://application:,,,/EKO;component/model/HQ.png"));
            }
            IMG.Source = bitmapImage;
        }

        private void calc_Click(object sender, RoutedEventArgs e)
        {
            if (enterprise.SelectedIndex == -1 || pollutant.SelectedIndex == -1)
            {
                return;
            }
            OUTDataGrid.Visibility = Visibility.Visible;
        }
    }
}
