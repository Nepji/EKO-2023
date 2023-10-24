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
using System.Windows.Shapes;

namespace EKO.window
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PollutantAddWin : Window
    {
        private bool EditMode = false;
        private int EditID;
        public PollutantAddWin()
        {
            InitializeComponent();
            Delete.Visibility = Visibility.Hidden;
        }

        public PollutantAddWin(string[] EditValue)
        {
            InitializeComponent();
            EditID = int.Parse(EditValue[0]);
            Value1.Text = EditValue[1];
            Value2.Text = EditValue[2];
            Value3.Text = EditValue[3];
            Value4.Text = EditValue[4];
            Value5.Text = EditValue[5];
            Add.Content = "Edit";
            Delete.Visibility = Visibility.Visible;
            EditMode = true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Value1.Text) || string.IsNullOrEmpty(Value2.Text) || string.IsNullOrEmpty(Value3.Text) || string.IsNullOrEmpty(Value4.Text) || string.IsNullOrEmpty(Value5.Text) || string.IsNullOrEmpty(Value6.Text) || string.IsNullOrEmpty(Value7.Text))
            {
                Notify.Text = "All lines must be filled!";
                return;
            }
            if (EditMode == true)
            {
                EditModeFunction();
                return;
            }
            string cmd = "INSERT INTO pollutant (`Name`,`MIn Mass Expenditure`,`Max Mass Expenditure`,`TLK`,`RFC`,`SFI`,`Danger class`) VALUES (@param1, @param2,@param3,@param4,@param5,@param6,@param7)";
            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(cmd, connection))
            {
                command.Parameters.AddWithValue("@param1", Value1.Text);
                command.Parameters.AddWithValue("@param2", Value2.Text);
                command.Parameters.AddWithValue("@param3", Value3.Text);
                command.Parameters.AddWithValue("@param4", Value3.Text);
                command.Parameters.AddWithValue("@param5", Value3.Text);
                command.Parameters.AddWithValue("@param6", Value3.Text);
                command.Parameters.AddWithValue("@param7", Value3.Text);
                command.ExecuteNonQuery();
                Notify.Text = "Success!";
            }
            connection.Close();
        }

        private void EditModeFunction()
        {
            DeleteFunction();
            string cmd = "INSERT INTO pollutant (`Id`,`Name`,`MIn Mass Expenditure`,`Max Mass Expenditure`,`TLK`,`RFC`,`SFI`,`Danger class`) VALUES (@param0,@param1, @param2,@param3,@param4,@param5,@param6,@param7)";
            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(cmd, connection))
            {
                command.Parameters.AddWithValue("@param0", EditID);
                command.Parameters.AddWithValue("@param1", Value1.Text);
                command.Parameters.AddWithValue("@param2", Value2.Text);
                command.Parameters.AddWithValue("@param3", Value3.Text);
                command.Parameters.AddWithValue("@param4", Value3.Text);
                command.Parameters.AddWithValue("@param5", Value3.Text);
                command.Parameters.AddWithValue("@param6", Value3.Text);
                command.Parameters.AddWithValue("@param7", Value3.Text);

                command.ExecuteNonQuery();
                Notify.Text = "Success!";
            }
            connection.Close();
        }

        private void DeleteFunction()
        {
            string cmd = $"DELETE FROM {model.DataBase.getInstance()._currentTable} WHERE `Id` = {EditID};";
            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand(cmd, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteFunction();
            Close();
        }
    }
}
