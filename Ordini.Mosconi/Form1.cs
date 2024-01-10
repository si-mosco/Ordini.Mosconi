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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String ConnectionString = "server=127.0.0.1;uid=Utente1;pwd=password;database=ordini";
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();

            String sql1 = "select * from clienti;";
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
            cmd1.ExecuteNonQuery();
            MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
            MyAdapter.SelectCommand = cmd1;
            DataTable dati = new DataTable();
            MyAdapter.Fill(dati);



            String sql2 = "select * from ordini;";
            cmd1 = new MySqlCommand(sql2, conn);
            cmd1.ExecuteNonQuery();
            MyAdapter = new MySqlDataAdapter();
            MyAdapter.SelectCommand = cmd1;
            DataTable dati2 = new DataTable();
            MyAdapter.Fill(dati2);

            dataGridView1.DataSource = dati;
            dataGridView2.DataSource = dati2;
            conn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabControl1.SelectedIndex = 2;
        }
    }
}
