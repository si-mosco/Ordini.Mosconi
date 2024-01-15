using System;
using System.Collections;
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

namespace Ordini.Mosconi {
    public partial class Form1 : Form {
        public DataTable d1 = new DataTable();
        public DataTable d2 = new DataTable();
        public DataTable d3 = new DataTable();
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false; //toglie ultima riga vuota
            dataGridView1.MultiSelect = false; //toglie la possibilità di selezionare più righe insieme
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.MultiSelect = false;
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.MultiSelect = false;
            tabControl1.SelectedIndex = 3;
            comboBox1.SelectedIndex = 0;

            d1= Query("select * from clienti;");
            d2= Query("select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;");
            d3= Query("select * from oggetti;");

            Aggiorna();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) //aggiorno tutte le volte che cambio tab
        {
            Aggiorna();
        }

        private void button1_Click(object sender, EventArgs e) //bottone aggionrna DA RIVEDERE
        {
            Aggiorna();
        }

        private void Aggiorna() //riprende i valori direttamente dal database
        {
            String ConnectionString = "server=127.0.0.1;uid=Utente1;pwd=password;database=ordini";
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();

            AggiornaGridView();

            comboBox1.SelectedItem = comboBox1.Items[0];//azzero i valori
            comboBox2.Visible = false;
            comboBox3.SelectedItem = comboBox3.Items[0];
            comboBox4.Visible = false;
            comboBox5.SelectedItem = comboBox5.Items[0];
            comboBox6.Visible = false;
            comboBox7.Visible = false;
            numericUpDown1.Visible = false;

            conn.Close();
        }

        public void AggiornaGridView() {
            dataGridView1.DataSource = d1;
            dataGridView2.DataSource = d2;
            dataGridView3.DataSource = d3;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //in base alla selezione cambio i valori della combobox successiva
        {
            comboBox2.Visible = true;
            switch (comboBox1.SelectedIndex) {
                case 0:
                    comboBox2.Visible = false;
                    d1 = Query("select * from clienti;");

                    break;

                case 1:
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange(DataTableToStringArray(Query("select id from clienti")).Distinct().ToArray());

                    break;

                case 2:
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange(DataTableToStringArray(Query("select nome from clienti")).Distinct().ToArray());

                    break;

                case 3:
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange(DataTableToStringArray(Query("select cognome from clienti")).Distinct().ToArray());

                    break;

                case 4:
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange(DataTableToStringArray(Query("select email from clienti")).Distinct().ToArray());

                    break;
            }
            AggiornaGridView();
        }

        public static string[] DataTableToStringArray(DataTable dt) //converte il risultato di una query in un array di stringhe
        {
            string[] result = new string[dt.Rows.Count];
            int index = 0;

            foreach (DataRow row in dt.Rows) {
                result[index] = string.Join(" ", row.ItemArray);
                index++;
            }

            return result;
        }

        public DataTable Query(string query) { //esegue le query
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

        public void ShortQuery(string query) {
            String ConnectionString = "server=localhost;uid=Utente1;pwd=password;database=ordini";
            MySqlConnection conn = new MySqlConnection(ConnectionString);

            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { //filtro clienti
            string[] valori = new string[] { "id", "nome", "cognome", "email" };
            d1 = Query($"select * from clienti where clienti.{valori[comboBox1.SelectedIndex - 1]} = '{comboBox2.GetItemText(comboBox2.SelectedItem)}';");

            AggiornaGridView();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e) { //in base alla selezione cambio i valori della combobox successiva
            comboBox6.Visible = true;
            comboBox7.Visible = false;
            numericUpDown1.Visible = false;
            switch (comboBox5.SelectedIndex) {
                case 0:
                    comboBox6.Visible = false;
                    d3 = Query("select * from oggetti;");

                    break;

                case 1:
                    comboBox6.Items.Clear();
                    comboBox6.Items.AddRange(DataTableToStringArray(Query("select id from oggetti")).Distinct().ToArray());

                    break;

                case 2:
                    comboBox6.Items.Clear();
                    comboBox6.Items.AddRange(DataTableToStringArray(Query("select nome from oggetti")).Distinct().ToArray());

                    break;

                case 3:
                    comboBox6.Items.Clear();
                    comboBox6.Visible = false;
                    comboBox7.Visible = true;
                    numericUpDown1.Visible = true;

                    break;
            }
            AggiornaGridView();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e) { //filtro oggetto
            string[] valori = new string[] { "id", "nome", "costo" };

            if (comboBox5.SelectedIndex != 3)
                d3 = Query($"select * from oggetti where oggetti.{valori[comboBox5.SelectedIndex - 1]} = '{comboBox6.GetItemText(comboBox6.SelectedItem)}';");
            
            AggiornaGridView();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) { //in base alla selezione cambio i valori della combobox successiva
            comboBox4.Visible = true;
            switch (comboBox3.SelectedIndex) {
                case 0:
                    comboBox4.Visible = false;
                    d2 = Query("select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;");

                    break;

                case 1:
                    comboBox4.Items.Clear();
                    comboBox4.Items.AddRange(DataTableToStringArray(Query("select ordini.id from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;")).Distinct().ToArray());

                    break;

                case 2:
                    comboBox4.Items.Clear();
                    comboBox4.Items.AddRange(DataTableToStringArray(Query("select cliente_id from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;")).Distinct().ToArray());

                    break;

                case 3:
                    comboBox4.Items.Clear();
                    comboBox4.Items.AddRange(DataTableToStringArray(Query("select data_ordine from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;")).Distinct().ToArray());

                    break;

                case 4:
                    comboBox4.Items.Clear();
                    comboBox4.Items.AddRange(DataTableToStringArray(Query("select oggetto_id from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;")).Distinct().ToArray());

                    break;
            }
            AggiornaGridView();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e) { //filtro per gli ordini
            string[] valori = new string[] { "id", "cliente_id", "data_ordine", "oggetto_id" };

            if (comboBox3.SelectedIndex - 1 == 0)
                d2 = Query($"select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id where ordini.{valori[comboBox3.SelectedIndex - 1]} = '{comboBox4.GetItemText(comboBox4.SelectedItem)}';");
            else if (comboBox3.SelectedIndex - 1 == 1 || comboBox3.SelectedIndex - 1 == 3)
                d2 = Query($"select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id where {valori[comboBox3.SelectedIndex - 1]} = '{comboBox4.GetItemText(comboBox4.SelectedItem)}';");
            else if (comboBox3.SelectedIndex - 1 == 2) {
                string date = comboBox4.Items[comboBox4.SelectedIndex].ToString();

                date = date.Split(' ')[0]; //modifico formato data
                date = date.Split('/')[2] + date.Split('/')[1] + date.Split('/')[0];

                d2 = Query($"select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id where ordini.{valori[comboBox3.SelectedIndex - 1]} = '{date}';");
            }
            AggiornaGridView();
        }

        private void button2_Click(object sender, EventArgs e) {
            Elimina();
        }

        private void Elimina() {
            int selezionati = 0;
            string pk="";
            string table = "";
            if (tabControl1.SelectedIndex == 0) {
                table = "clienti";
                selezionati = dataGridView1.SelectedRows.Count;
                if (selezionati > 0)
                    pk = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                else {
                    MessageBox.Show("Selezionare un elemento");
                    return;
                }
            } else if (tabControl1.SelectedIndex == 1) {
                table = "ordini";
                selezionati = dataGridView2.SelectedRows.Count;
                if (selezionati > 0)
                    pk = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
                else {
                    MessageBox.Show("Selezionare un elemento");
                    return;
                }
            } else if (tabControl1.SelectedIndex == 2) {
                table = "oggetti";
                selezionati = dataGridView3.SelectedRows.Count;
                if (selezionati > 0)
                    pk = dataGridView3.SelectedRows[0].Cells[0].Value.ToString();
                else {
                    MessageBox.Show("Selezionare un elemento");
                    return;
                }
            }

            var ris = MessageBox.Show("", "Conferma cancellazione", MessageBoxButtons.YesNo);
            try {
                if (ris == DialogResult.Yes)
                    ShortQuery($"delete from {table} where id='{pk}';");
            } catch {
                MessageBox.Show("L'elemento non può essere eliminato, visto che è associato ad un altro elemento");
            }
            AggiornaGridView();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e) {
            if (comboBox5.SelectedIndex == 3)
                d3 = Query($"select * from oggetti where oggetti.costo {comboBox7.GetItemText(comboBox7.SelectedItem)} {numericUpDown1.Value};");

            AggiornaGridView();
        }

        private void button3_Click(object sender, EventArgs e) {
            Elimina();
        }

        private void button4_Click(object sender, EventArgs e) {
            Elimina();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
            comboBox7_SelectedIndexChanged(sender, e);
        }

        private void button5_Click(object sender, EventArgs e) { //aggiungi cliente
            string nome = "";
            string cognome = "";
            string email = "";

            bool doit = true;
            if (!String.IsNullOrWhiteSpace(textBox1.Text)) {
                nome = textBox1.Text;
            } else {
                MessageBox.Show("Inserire un nome valido");
                textBox1.Text = "";
                doit = false;
            }
            if (!String.IsNullOrWhiteSpace(textBox2.Text)) {
                cognome = textBox2.Text;
            } else {
                MessageBox.Show("Inserire un cognome valido");
                textBox2.Text = "";
                doit = false;
            }
            if (!String.IsNullOrWhiteSpace(textBox3.Text)) {
                email = textBox3.Text;
            } else {
                MessageBox.Show("Inserire una email valida");
                textBox3.Text = "";
                doit = false;
            }

            if (doit) {
                ShortQuery($"insert into `clienti`(`nome`, `cognome`, `email`) values(\"{nome}\", \"{cognome}\", \"{email}\")");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }

            AggiornaGridView();
        }

        private void button6_Click(object sender, EventArgs e) { //aggiungi oggetto
            string nome = "";
            int costo = 0;

            bool doit = true;
            if (!String.IsNullOrWhiteSpace(textBox6.Text)) {
                nome = textBox6.Text;
            } else {
                MessageBox.Show("Inserire un nome valido");
                textBox6.Text = "";
                doit = false;
            }
            if (numericUpDown2.Value>0) {
                costo = (int)numericUpDown2.Value;
            } else {
                MessageBox.Show("Inserire un costo valido");
                numericUpDown2.Value = 10;
                doit = false;
            }

            if (doit) {
                ShortQuery($"insert into `oggetti`(`nome`, `costo`) values(\"{nome}\", \"{costo}\")");
                textBox6.Text = "";
                numericUpDown2.Value = 10;
            }

            AggiornaGridView();
        }
    }
}