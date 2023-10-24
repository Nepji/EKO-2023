using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;


namespace EKO.window
{
    /// <summary>
    /// Interaction logic for PollutionEditWin.xaml
    /// </summary>
    public partial class PollutionEditWin : Window
    {
        private bool EditMode = false;
        private int EditID;
        public PollutionEditWin()
        {
            InitializeComponent();
            LoadData();
            Delete.Visibility = Visibility.Hidden;
        }

        public PollutionEditWin(string[] EditValue)
        {
            InitializeComponent();
            LoadData();

            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();

            string cmdtext = "select `Id`,`Name` from enterprise where Id = " + EditValue[1];
            MySqlCommand command = new MySqlCommand(cmdtext, connection);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Value1.Text = $"{reader[0]}:{reader[1]}";
            command.CommandText = "select `Id`,`Name` from pollutant where Id = " + EditValue[2];
            reader.Close();
            reader = command.ExecuteReader();
            reader.Read();
            Value2.Text = $"{reader[0]}:{reader[1]}"; ;
            Value3.Text = EditValue[3];
            Value4.Text = EditValue[4];
            EditID = int.Parse(EditValue[0]);
            Add.Content = "Edit";
            Delete.Visibility = Visibility.Visible;
            EditMode = true;
        }

        private void LoadData()
        {
            string cmdtext = "select `Id`,`Name` from pollutant";
            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand(cmdtext, connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Value2.Items.Add($"{reader[0]}:{reader[1]}");
                }
            }
            command.CommandText = "select `Id`,`Name` from enterprise";
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Value1.Items.Add($"{reader[0]}:{reader[1]}");
                }
            }

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Value1.Text) || string.IsNullOrEmpty(Value2.Text) || string.IsNullOrEmpty(Value3.Text) || string.IsNullOrEmpty(Value4.Text))
            {
                Notify.Text = "All lines must be filled!";
                return;
            }
            if (EditMode == true)
            {
                EditModeFunction();
                return;
            }
            string cmd = "INSERT INTO pollution (`Enterprise`,`Pollutant`,`Number of Emissions`,`Year`) VALUES (@param1, @param2,@param3,@param4)";
            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(cmd, connection))
            {
                command.Parameters.AddWithValue("@param1", Value1.Text.Split(':')[0]);
                command.Parameters.AddWithValue("@param2", Value2.Text.Split(':')[0]);
                command.Parameters.AddWithValue("@param3", Value3.Text);
                command.Parameters.AddWithValue("@param4", Convert.ToInt32(Value4.Text));

                command.ExecuteNonQuery();
                Notify.Text = "Success!";
            }
            connection.Close();
        }

        private void EditModeFunction()
        {
            DeleteFunction();
            string cmd = "INSERT INTO pollution (`Id`,`Enterprise`,`Pollutant`,`Number of Emissions`,`Year`) VALUES (@param0,@param1, @param2,@param3,@param4)";
            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(cmd, connection))
            {
                command.Parameters.AddWithValue("@param0", EditID);
                command.Parameters.AddWithValue("@param1", Value1.Text.Split(':')[0]);
                command.Parameters.AddWithValue("@param2", Value2.Text.Split(':')[0]);
                command.Parameters.AddWithValue("@param3", Value3.Text);
                command.Parameters.AddWithValue("@param4", Value3.Text);

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
