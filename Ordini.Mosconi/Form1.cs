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
    }
}
