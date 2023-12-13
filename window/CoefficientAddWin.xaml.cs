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
    /// Interaction logic for CoefficientAddWin.xaml
    /// </summary>
    public partial class CoefficientAddWin : Window
    {
        private bool EditMode = false;
        private int EditID;
        public CoefficientAddWin()
        {
            InitializeComponent();
            Delete.Visibility = Visibility.Hidden;
            EditMode = false;
        }

        public CoefficientAddWin(string[] EditValue)
        {
            InitializeComponent();
            EditID = int.Parse(EditValue[0]);
            Name.Text = EditValue[1];
            Pv.Text = EditValue[2];
            Kav.Text = EditValue[3];
            Lvv.Text = EditValue[4];
            Pnv.Text = EditValue[5];
            knv.Text = EditValue[6];
            Lnv.Text = EditValue[7];
            Pcost.Text = EditValue[8];
            OPT_SG.Text = EditValue[9];
            SSG.Text = EditValue[10];
            kps.Text = EditValue[11];
            OU.Text = EditValue[12];
            OPTcostU.Text = EditValue[13];
            vo.Text = EditValue[14];
            OCostM.Text = EditValue[15];
            OMLost.Text = EditValue[16];
            KVPP.Text = EditValue[17];
            SCostVM.Text = EditValue[18];
            KAM.Text = EditValue[19];
            IZC.Text = EditValue[20];
            kLostM.Text = EditValue[21];
            nzs.Text = EditValue[22];
            kfpu.Text = EditValue[23];
            kfpl.Text = EditValue[24];

            Add.Content = "Edit";
            Delete.Visibility = Visibility.Visible;
            EditMode = true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (EditMode == true)
            {
                EditModeFunction();
                return;
            }
            AddFuncrion();

        }
        private void AddFuncrion()
        {
            string cmd = $"SET FOREIGN_KEY_CHECKS=0;INSERT INTO `damagescf` (`id`, `Name`, `Р(вир)`, `к-амортизації(вир)`, `Лв(вир)`, `Р(невир)`, `к-амортизації(невир)`, `Лв(невир)`, `вартість продукції`, `опт.ціна С/Г продукції`, `Sпошк С/Г культ`, `k-пошк посівів`, `очік урожай`, `опт ціна урожаю`, `витрати обсягу`, `опт ціна матеріалів`, `обсяг втрачених матеріалів`, `к-сть втрач проміж прод`, `баланс варть втрач майна`, `К-А майна`, `індекс зміни цін`, `к-сть втрач майна`, `норматив збитків`, `к-ф знпродук угіддя`, `к-ф прод лісів`)  VALUES (@param0,@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12,@param13,@param14,@param15,@param16,@param17,@param18,@param19,@param20,@param21,@param22,@param23,@param24)";
            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(cmd, connection))
            {
                command.Parameters.AddWithValue("@param0", EditID);
                command.Parameters.AddWithValue("@param1", Name.Text);
                command.Parameters.AddWithValue("@param2", Pv.Text);
                command.Parameters.AddWithValue("@param3", Kav.Text);
                command.Parameters.AddWithValue("@param4", Lvv.Text);
                command.Parameters.AddWithValue("@param5", Pnv.Text);
                command.Parameters.AddWithValue("@param6", knv.Text);
                command.Parameters.AddWithValue("@param7", Lnv.Text);
                command.Parameters.AddWithValue("@param8", Pcost.Text);
                command.Parameters.AddWithValue("@param9", OPT_SG.Text);
                command.Parameters.AddWithValue("@param10", SSG.Text);
                command.Parameters.AddWithValue("@param11", kps.Text);
                command.Parameters.AddWithValue("@param12", OU.Text);
                command.Parameters.AddWithValue("@param13", OPTcostU.Text);
                command.Parameters.AddWithValue("@param14", vo.Text);
                command.Parameters.AddWithValue("@param15", OCostM.Text);
                command.Parameters.AddWithValue("@param16", OMLost.Text);
                command.Parameters.AddWithValue("@param17", KVPP.Text);
                command.Parameters.AddWithValue("@param18", SCostVM.Text);
                command.Parameters.AddWithValue("@param19", KAM.Text);
                command.Parameters.AddWithValue("@param20", IZC.Text);
                command.Parameters.AddWithValue("@param21", kLostM.Text);
                command.Parameters.AddWithValue("@param22", nzs.Text);
                command.Parameters.AddWithValue("@param23", kfpu.Text);
                command.Parameters.AddWithValue("@param24", kfpl.Text);


                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        private void EditModeFunction()
        {
            DeleteFunction();
            AddFuncrion();
            
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
