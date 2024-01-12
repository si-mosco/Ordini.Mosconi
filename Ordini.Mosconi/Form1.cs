using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.Relational;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Ordini.Mosconi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabControl1.SelectedIndex = 3;
            comboBox1.SelectedIndex = 0;

            Aggiorna();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Aggiorna();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Aggiorna();
        }

        private void Aggiorna()
        {
            String ConnectionString = "server=localhost;uid=Utente1;pwd=password;database=ordini";
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            
            dataGridView1.DataSource = Query("select * from clienti;");
            dataGridView2.DataSource = Query("select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;");
            dataGridView3.DataSource = Query("select * from oggetti;");
            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Visible = true;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    comboBox2.Visible=false;
                    dataGridView1.DataSource = Query("select * from clienti;");

                    break;

                case 1:
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange(DataTableToStringArray(Query("select id from clienti")));

                    break;

                case 2:
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange(DataTableToStringArray(Query("select nome from clienti")));

                    break;

                case 3:
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange(DataTableToStringArray(Query("select cognome from clienti")));

                    break;

                case 4:
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange(DataTableToStringArray(Query("select email from clienti")));

                    break;
            }
        }

        public static string[] DataTableToStringArray(DataTable dt)
        {
            string[] result = new string[dt.Rows.Count];
            int index = 0;

            foreach (DataRow row in dt.Rows) {
                result[index] = string.Join(" ", row.ItemArray);
                index++;
            }

            return result;
        }

        private DataTable Query(string query) {
            String ConnectionString = "server=localhost;uid=Utente1;pwd=password;database=ordini";
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();

            String sql = query;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
            MyAdapter.SelectCommand = cmd;
            DataTable dati = new DataTable();
            MyAdapter.Fill(dati);

            conn.Close();

            return dati;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
            string[] valori = new string[] {"id", "nome", "cognome", "email"};

            //MessageBox.Show(valori[comboBox1.SelectedIndex-1] + " - " + comboBox1.SelectedIndex);
            //MessageBox.Show($"select * from clienti where {valori[comboBox1.SelectedIndex - 1]}={comboBox2.Items[comboBox2.SelectedIndex].ToString()};");
            
            dataGridView1.DataSource = Query($"select * from clienti where clienti.{valori[comboBox1.SelectedIndex - 1]} = '{comboBox2.Items[comboBox2.SelectedIndex]}';");
        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e) {
            comboBox6.Visible = true;
            switch (comboBox5.SelectedIndex) {
                case 0:
                    comboBox6.Visible = false;
                    dataGridView3.DataSource = Query("select * from oggetti;");

                    break;

                case 1:
                    comboBox6.Items.Clear();
                    comboBox6.Items.AddRange(DataTableToStringArray(Query("select id from oggetti")));

                    break;

                case 2:
                    comboBox6.Items.Clear();
                    comboBox6.Items.AddRange(DataTableToStringArray(Query("select nome from oggetti")));

                    break;

                case 3:
                    comboBox6.Items.Clear();
                    comboBox6.Items.AddRange(DataTableToStringArray(Query("select costo from oggetti")));

                    break;
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e) {
            string[] valori = new string[] { "id", "nome", "costo"};

            dataGridView3.DataSource = Query($"select * from oggetti where oggetti.{valori[comboBox5.SelectedIndex - 1]} = '{comboBox6.Items[comboBox6.SelectedIndex]}';");
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) {
            comboBox4.Visible = true;
            switch (comboBox3.SelectedIndex) {
                case 0:
                    comboBox4.Visible = false;
                    dataGridView2.DataSource = Query("select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;");

                    break;

                case 1:
                    comboBox4.Items.Clear();
                    comboBox4.Items.AddRange(DataTableToStringArray(Query("select ordini.id from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;")));

                    break;

                case 2:
                    comboBox4.Items.Clear();
                    comboBox4.Items.AddRange(DataTableToStringArray(Query("select cliente_id from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;")));

                    break;

                case 3:
                    comboBox4.Items.Clear();
                    comboBox4.Items.AddRange(DataTableToStringArray(Query("select data_ordine from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;")));

                    break;

                case 4:
                    comboBox4.Items.Clear();
                    comboBox4.Items.AddRange(DataTableToStringArray(Query("select oggetto_id from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;")));

                    break;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e) {
            string[] valori = new string[] { "id", "cliente_id", "data_ordine", "oggetto_id" };

            if (comboBox3.SelectedIndex - 1 == 0)
                dataGridView2.DataSource = Query($"select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id where ordini.{valori[comboBox3.SelectedIndex - 1]} = '{comboBox4.Items[comboBox4.SelectedIndex]}';");
            else if (comboBox3.SelectedIndex - 1 == 1 || comboBox3.SelectedIndex - 1 == 3)
                dataGridView2.DataSource = Query($"select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id where {valori[comboBox3.SelectedIndex - 1]} = '{comboBox4.Items[comboBox4.SelectedIndex]}';");
            else if (comboBox3.SelectedIndex - 1 == 2) {
                string date = comboBox4.Items[comboBox4.SelectedIndex].ToString();

                date = date.Split(' ')[0]; //modifico formato data
                date = date.Split('/')[2] + date.Split('/')[1] + date.Split('/')[0];

                dataGridView2.DataSource = Query($"select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id where ordini.{valori[comboBox3.SelectedIndex - 1]} = '{date}';");
            }
        }
    }
}
