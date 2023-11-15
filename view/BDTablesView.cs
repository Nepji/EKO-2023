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
            switch (model.DataBase.getInstance()._currentTable)
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
                case model.TablesNames.dangerclass:
                    new window.DangerClass().Show();
                    break;
                case model.TablesNames.tax:
                    MessageBox.Show("Taxes cannot be added manually!");
                    break;
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            LoadFunction();
        }

        private void LoadFunction()
        {
            connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            string query = "";
            if (model.DataBase.getInstance()._currentTable == model.TablesNames.pollution)
            {
                query = "select pollution.id, enterprise.Name as `Enterprise Name`, pollutant.Name as `Pollutant Name`, pollution.`Number of Emissions`,pollution.CR,pollution.CR_impact,pollution.HQ,pollution.HQ_impact,pollution.`Year` from enterprise Inner join pollution on enterprise.id = pollution.Enterprise INNER join pollutant on pollutant.id = pollution.Pollutant; ";
            }
            else if (model.DataBase.getInstance()._currentTable == model.TablesNames.tax)
            {
                query = "select tax.id, enterprise.Name as `Enterprise Name`, pollutant.Name as `Pollutant Name`, pollution.Year, tax.tax,tax.detriment from enterprise Inner join tax on enterprise.id = (select Enterprise from pollution where pollution.id = tax.id) INNER join pollutant on pollutant.id = (select Pollutant from pollution where pollution.id = tax.id) INNER join pollution on pollution.id = tax.id;";
            }
            else
            {
                query = "SELECT * FROM " + model.DataBase.getInstance()._currentTable;
            }
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
                    case model.TablesNames.dangerclass:
                        new window.DangerClass(stringValues).Show();
                        break;
                    case model.TablesNames.tax:
                        MessageBox.Show("Taxes cannot be changed!");
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
                        string insertQuery = null;
                        string column1Value = null;
                        string column2Value = null;
                        string column3Value = null;
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
                            case model.TablesNames.pollution:
                                {
                                    column1Value = worksheet.Cells[row, 1].Text;
                                    column2Value = worksheet.Cells[row, 2].Text;
                                    column3Value = worksheet.Cells[row, 3].Text.Replace(",", ".");
                                    column4Value = worksheet.Cells[row, 4].Text;

                                    insertQuery =
                                        $"INSERT INTO pollution (`Enterprise`,`Pollutant`,`Number of Emissions`,`Year`) VALUES (@column1Value, @column2Value, @column3Value, @column4Value)";

                                    break;
                                }
                            case model.TablesNames.dangerclass:
                                {
                                    column1Value = worksheet.Cells[row, 1].Text;
                                    column2Value = worksheet.Cells[row, 2].Text.Replace(",", ".");

                                    insertQuery =
                                        $"INSERT INTO dangercalss (`Name`,`TaxRate`) VALUES (@column1Value, @column2Value)";

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
