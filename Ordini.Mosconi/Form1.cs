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
            tabControl1.SelectedIndex = 2;
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
            MySqlCommand cmd1;
            MySqlDataAdapter MyAdapter;
            DataTable dati = null;
            String sql1 = "";
            DataTable dati2 = null;

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    sql1 = "select * from clienti;";
                    cmd1 = new MySqlCommand(sql1, conn);
                    cmd1.ExecuteNonQuery();
                    MyAdapter = new MySqlDataAdapter();
                    MyAdapter.SelectCommand = cmd1;
                    dati = new DataTable();
                    MyAdapter.Fill(dati);

                    break;

                case 1:
                    sql1 = "select * from clienti;";
                    cmd1 = new MySqlCommand(sql1, conn);
                    cmd1.ExecuteNonQuery();
                    MyAdapter = new MySqlDataAdapter();
                    MyAdapter.SelectCommand = cmd1;
                    dati = new DataTable();
                    MyAdapter.Fill(dati);

                    break;

                case 2:
                    sql1 = "select * from clienti;";
                    cmd1 = new MySqlCommand(sql1, conn);
                    cmd1.ExecuteNonQuery();
                    MyAdapter = new MySqlDataAdapter();
                    MyAdapter.SelectCommand = cmd1;
                    dati = new DataTable();
                    MyAdapter.Fill(dati);

                    break;

                case 3:
                    sql1 = "select * from clienti;";
                    cmd1 = new MySqlCommand(sql1, conn);
                    cmd1.ExecuteNonQuery();
                    MyAdapter = new MySqlDataAdapter();
                    MyAdapter.SelectCommand = cmd1;
                    dati = new DataTable();
                    MyAdapter.Fill(dati);

                    break;

                case 4:
                    sql1 = "select * from clienti;";
                    cmd1 = new MySqlCommand(sql1, conn);
                    cmd1.ExecuteNonQuery();
                    MyAdapter = new MySqlDataAdapter();
                    MyAdapter.SelectCommand = cmd1;
                    dati = new DataTable();
                    MyAdapter.Fill(dati);

                    break;
            }


            String sql2 = "select ordini.id, email, ordini.data_ordine, ordini.importo, ordini.oggetto from clienti join ordini on clienti.id=ordini.cliente_id;";
            cmd1 = new MySqlCommand(sql2, conn);
            cmd1.ExecuteNonQuery();
            MyAdapter = new MySqlDataAdapter();
            MyAdapter.SelectCommand = cmd1;
            dati2 = new DataTable();
            MyAdapter.Fill(dati2);


            dataGridView1.DataSource = dati;
            dataGridView2.DataSource = dati2;
            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Visible = true;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    comboBox2.Visible=false;
                    break;

                case 1:
                    comboBox2.Items.Clear();

                    String ConnectionString = "server=localhost;uid=Utente1;pwd=password;database=ordini";
                    MySqlConnection conn = new MySqlConnection(ConnectionString);
                    conn.Open();

                    String sql = "select cognome from clienti";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                    MyAdapter.SelectCommand = cmd;
                    DataTable dati = new DataTable();
                    MyAdapter.Fill(dati);

                    conn.Close();

                    comboBox2.Items.Add(DataTableToStringArray(dati));

                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    break;
            }
        }

        public static string[] DataTableToStringArray(DataTable dt)
        {
            return dt.AsEnumerable().Skip(1).Select(row => row.Field<string>("Names")).ToArray();
        }
    }
}
