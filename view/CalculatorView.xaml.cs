using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EKO.view
{
    /// <summary>
    /// Interaction logic for CalculatorView.xaml
    /// </summary>
    public partial class CalculatorView : UserControl
    {

        private bool StatusButton = true;
        private bool StatusRButton = true;
        string enterprisetext;
        string scftext;
        int child = 7;

        public CalculatorView()
        {
            InitializeComponent();
            Mp.IsReadOnly = true;
            Hp.IsReadOnly = true;
            total.IsReadOnly = true;
            Рл_г.IsReadOnly = true;
            Рс_г.IsReadOnly = true;
            PreLoad();
        }


        private void PreLoad()
        {
            String cmdtext = "select `Id`,`Name` from enterprise";
            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand(cmdtext, connection);
            
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    enterprise.Items.Add($"{reader[0]}:{reader[1]}");
                }
            }
            command.CommandText = "select `Id`,`Name` from damagescf";
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    coefficients.Items.Add($"{reader[0]}:{reader[1]}");
                }
            }
        }

        private void AddCFButton_Click(object sender, RoutedEventArgs e)
        {
            if (StatusButton)
            {
                AddCFButton.Content = "Refresh";
                StatusButton = false;
                new window.CoefficientAddWin().Show();

            }
            else
            {
                coefficients.Items.Clear();
                PreLoad();
                AddCFButton.Content = "Add";
                StatusButton = true;
            }

        }

        private void calc_Click(object sender, RoutedEventArgs e)
        {
           enterprisetext = enterprise.Text.Split(':')[0];
           scftext = coefficients.Text.Split(':')[0];
           total.Text = CalctoTotal().ToString();
        }


        private float CalcHp()
        {
            float result = 0;
            float SVtpp = (float)(float.Parse(kpl.Text)*280 + float.Parse(kph.Text)*6500 + float.Parse(kpi.Text)*37000 + float.Parse(kd.Text)*47000);
            float SVDDP = 12 * 150 * float.Parse(kd.Text);
            float Svvtg = 12 * 37 * (18 - child);
            result = SVtpp + SVDDP + Svvtg;
            Hp.Text = result.ToString();
            return result;

        }

        private float CalcMp()
        {
            
            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            
            float result = 0;

            /////////////////////////////////////
            MySqlCommand command = new MySqlCommand($"select `Р(вир)` from damagescf where id = {scftext}", connection);
            float Pv = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `к-амортизації(вир)` from damagescf where id = {scftext}";
            float kav = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `Лв(вир)` from damagescf where id = {scftext}";
            float lvv = float.Parse(command.ExecuteScalar().ToString());
            float Fv = Pv + kav - lvv;
            /////////////////////////////////////
            command.CommandText = $"select `Р(невир)` from damagescf where id = {scftext}";
            float pnv = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `к-амортизації(невир)` from damagescf where id = {scftext}";
            float kanv = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `Лв(невир)` from damagescf where id = {scftext}";
            float lnvv = float.Parse(command.ExecuteScalar().ToString());
            float Fg = pnv+kanv-lnvv;
            /////////////////////////////////////
            command.CommandText = $"select `вартість продукції` from damagescf where id = {scftext}";
            float Pr = float.Parse(command.ExecuteScalar().ToString());
            /////////////////////////////////////
            command.CommandText = $"select `опт.ціна С/Г продукції` from damagescf where id = {scftext}";
            float opcsg = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `Sпошк С/Г культ` from damagescf where id = {scftext}";
            float spsg = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `k-пошк посівів` from damagescf where id = {scftext}";
            float kpp = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `очік урожай` from damagescf where id = {scftext}";
            float ou = float.Parse(command.ExecuteScalar().ToString());
            float Prc = opcsg * spsg * kpp - ou;
            /////////////////////////////////////
            command.CommandText = $"select `опт ціна урожаю` from damagescf where id = {scftext}";
            float ci = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `витрати обсягу` from damagescf where id = {scftext}";
            float qi = float.Parse(command.ExecuteScalar().ToString());
            float Cn = ci*qi;
            /////////////////////////////////////
            command.CommandText = $"select `опт ціна матеріалів` from damagescf where id = {scftext}";
            float ocm = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `обсяг втрачених матеріалів` from damagescf where id = {scftext}";
            float ovm = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `к-сть втрач проміж прод` from damagescf where id = {scftext}";
            float kvpp = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `баланс варть втрач майна` from damagescf where id = {scftext}";
            float bvvm = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `К-А майна` from damagescf where id = {scftext}";
            float kam = float.Parse(command.ExecuteScalar().ToString());

            command.CommandText = $"select `індекс зміни цін` from damagescf where id = {scftext}";
            float izc = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `к-сть втрач майна` from damagescf where id = {scftext}";
            float kvm = float.Parse(command.ExecuteScalar().ToString());
            float Mdg = ocm*ovm*kvpp*bvvm*kam - izc*kvm;
            /////////////////////////////////////

            result = Fv + Fg + Pr + Prc + Cn ;
            Mp.Text = result.ToString();
            connection.Close();
            return result;
        }

        private float CalcРс()
        {
            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand($"select `норматив збитків` from damagescf where id = {scftext}", connection);
            float nzs = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `к-ф знпродук угіддя` from damagescf where id = {scftext}";
            float kzpu = float.Parse(command.ExecuteScalar().ToString());
            float РС = float.Parse(ssg.Text);
            float result = Math.Abs(РС * nzs + РС * 160 * (1 - kzpu));
            Рс_г.Text = result.ToString();
            return result;
        }
        private float CalcРл()
        {
            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand($"select `к-ф прод лісів` from damagescf where id = {scftext}", connection);
            float nzs = float.Parse(command.ExecuteScalar().ToString());
            command.CommandText = $"select `к-ф знпродук угіддя` from damagescf where id = {scftext}";
            float kzpu = float.Parse(command.ExecuteScalar().ToString());
            float РЛ = float.Parse(sld.Text);
            float result = Math.Abs(РЛ * nzs + РЛ * 123 * (1 - kzpu));
            Рл_г.Text = result.ToString();
            return result;
        }
        private float CalctoTotal()
        {
            float result = CalcMp() + CalcHp() + CalcРс() + CalcРл();
            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(total.Text))
            {
                MessageBox.Show("Need Smt to save!");
                return;
            }

            string cmd = "INSERT INTO `damagesave` (`id`, `enterpriseid`, `damagescfid`, `Mp`, `Hp`, `Рсг`, `Рлг`, `total`) VALUES (NULL, @param1, @param2, @param3, @param4, @param5, @param6, @param7);";
            MySqlConnection connection = new MySqlConnection(model.DataBase.getInstance().connectionString);
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(cmd, connection))
            {
                command.Parameters.AddWithValue("@param1", enterprisetext);
                command.Parameters.AddWithValue("@param2", scftext);
                command.Parameters.AddWithValue("@param3", Mp.Text);
                command.Parameters.AddWithValue("@param4", Hp.Text);
                command.Parameters.AddWithValue("@param5", Рс_г.Text);
                command.Parameters.AddWithValue("@param6", Рл_г.Text);
                command.Parameters.AddWithValue("@param7", total.Text);

                command.ExecuteNonQuery();
                MessageBox.Show("Success!");
            }
            connection.Close();
        }
    }
}
