using Microsoft.Win32;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EKO.view
{
    public partial class BDTablesView : UserControl
    {
        private MySqlConnection connection;
        private MySqlCommand cmd;
        private MySqlDataAdapter adapter;
        public BDTablesView()
        {
            InitializeComponent();
            BDDataGrid.IsReadOnly = true;
            LoadFunction();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            switch(model.DataBase.getInstance()._currentTable)
            {
                case model.TablesNames.enterprise:
                        new window.EnterpriseEditWin().Show();
                    break;
                case model.TablesNames.pollutant:
                    new window.PollutantAddWin().Show();
                    break;
                case model.TablesNames.pollution:
                    new window.PollutionEditWin().Show();
                    break;
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            LoadFunction();
        }

        public void LoadFunction()
        {
            connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            string query = "SELECT * FROM " + model.DataBase.getInstance()._currentTable;
            cmd = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            BDDataGrid.ItemsSource = dataTable.DefaultView;
            connection.Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = BDDataGrid.SelectedItem as DataRowView;

            if (selectedRow != null)
            {
                object[] values = selectedRow.Row.ItemArray;
                string[] stringValues = values.Select(value => value.ToString()).ToArray();
                switch (model.DataBase.getInstance()._currentTable)
                {
                    case model.TablesNames.enterprise:
                        new window.EnterpriseEditWin(stringValues).Show();
                        break;
                    case model.TablesNames.pollutant:
                        new window.PollutantAddWin(stringValues).Show();
                        break;
                    case model.TablesNames.pollution:
                        new window.PollutionEditWin(stringValues).Show();
                        break;
                }
            }
        }

        private void LoadFromExcel_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
            openFileDialog.Title = "Виберіть Excel-файл";
            string selectedFilePath = null;
            if (openFileDialog.ShowDialog() == true)
            {
                selectedFilePath = openFileDialog.FileName;
            }

            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            if (selectedFilePath != null)
            {
                using (var package = new ExcelPackage(new FileInfo(selectedFilePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        string insertQuery;
                        string column1Value;
                        string column2Value;
                        string column3Value;
                        string column4Value = null;
                        string column5Value = null;
                        switch (model.DataBase.getInstance()._currentTable)
                        {
                            case model.TablesNames.enterprise:
                                {
                                    column1Value = worksheet.Cells[row, 1].Text;
                                    column2Value = worksheet.Cells[row, 2].Text;
                                    column3Value = worksheet.Cells[row, 3].Text;

                                    insertQuery =
                                        $"INSERT INTO enterprise (`Name`,`Activity`,`Region`) VALUES (@column1Value, @column2Value, @column3Value)";
                                    break;
                                }
                            case model.TablesNames.pollutant:
                                {
                                    column1Value = worksheet.Cells[row, 1].Text;
                                    column2Value = worksheet.Cells[row, 2].Text == "Null"
                                        ? null
                                        : worksheet.Cells[row, 2].Text.Replace(",", ".");
                                    column3Value = worksheet.Cells[row, 3].Text == "Null"
                                        ? null
                                        : worksheet.Cells[row, 3].Text.Replace(",", ".");
                                    column4Value = worksheet.Cells[row, 4].Text == "Null"
                                        ? null
                                        : worksheet.Cells[row, 4].Text.Replace(",", ".");
                                    column5Value = worksheet.Cells[row, 5].Text == "Null"
                                        ? null
                                        : worksheet.Cells[row, 5].Text.Replace(",", ".");

                                    insertQuery =
                                        $"INSERT INTO pollutant (`Name`,`MIn Mass Expenditure`,`Max Mass Expenditure`,`TLK`,`Danger class`) VALUES (@column1Value, @column2Value, @column3Value, @column4Value, @column5Value)";
                                    break;
                                }
                            default:
                                {
                                    column1Value = worksheet.Cells[row, 1].Text;
                                    column2Value = worksheet.Cells[row, 2].Text;
                                    column3Value = worksheet.Cells[row, 3].Text.Replace(",", ".");
                                    column4Value = worksheet.Cells[row, 4].Text;

                                    insertQuery =
                                        $"INSERT INTO pollution (`Enterprise`,`Pollutant`,`Number of Emissions`,`Year`) VALUES (@column1Value, @column2Value, @column3Value, @column4Value)";

                                    break;
                                }
                        }

                        MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
                        MySqlCommand command = new MySqlCommand(insertQuery, connection);
                        connection.Open();
                        command.Parameters.AddWithValue("@column1Value", column1Value);
                        command.Parameters.AddWithValue("@column2Value", column2Value);
                        command.Parameters.AddWithValue("@column3Value", column3Value);
                        switch (model.DataBase.getInstance()._currentTable)
                        {
                            case model.TablesNames.pollutant:
                                command.Parameters.AddWithValue("@column4Value", column4Value);
                                command.Parameters.AddWithValue("@column5Value", column5Value);
                                break;
                            case model.TablesNames.pollution:
                                command.Parameters.AddWithValue("@column4Value", column4Value);
                                break;
                        }

                        try
                        {
                            
                            command.ExecuteNonQuery();
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }
                    }
                }
            }
        }
    }
}
