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
            dataGridView1.MultiSelect = false; //toglie la possibilità di selezionare più righe insieme
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = false;
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.MultiSelect = false;
            tabControl1.SelectedIndex = 3;
            comboBox1.SelectedIndex = 0;

            d1 = Query("select * from clienti;");
            d2 = Query("select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;");
            d3 = Query("select * from oggetti;");

            Aggiorna();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) //aggiorno tutte le volte che cambio tab
        {
            Aggiorna();
        }


        //tacControll pagina 1
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { //filtro clienti
            string[] valori = new string[] { "id", "nome", "cognome", "email" };
            d1 = Query($"select * from clienti where clienti.{valori[comboBox1.SelectedIndex - 1]} = '{comboBox2.GetItemText(comboBox2.SelectedItem)}';");

            AggiornaGridView();
        }

        private void button2_Click(object sender, EventArgs e) //elimina
        {
            Elimina();
        }

        private void button5_Click(object sender, EventArgs e)
        { //aggiungi cliente
            string nome = "";
            string cognome = "";
            string email = "";

            bool doit = true;
            if (!String.IsNullOrWhiteSpace(textBox1.Text))
            {
                nome = textBox1.Text;
            }
            else
            {
                MessageBox.Show("Inserire un nome valido");
                textBox1.Text = "";
                doit = false;
            }
            if (!String.IsNullOrWhiteSpace(textBox2.Text))
            {
                cognome = textBox2.Text;
            }
            else
            {
                MessageBox.Show("Inserire un cognome valido");
                textBox2.Text = "";
                doit = false;
            }
            if (!String.IsNullOrWhiteSpace(textBox3.Text))
            {
                email = textBox3.Text;
            }
            else
            {
                MessageBox.Show("Inserire una email valida");
                textBox3.Text = "";
                doit = false;
            }

            if (doit)
            {
                ShortQuery($"insert into `clienti`(`nome`, `cognome`, `email`) values(\"{nome}\", \"{cognome}\", \"{email}\")");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }

            d1 = Query("select * from clienti;");

            AggiornaGridView();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox8_SelectedIndexChanged(sender, e);
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        { // ordina clienti
            dataGridView1.Sort(dataGridView1.Columns[comboBox8.SelectedIndex], checkBox1.Checked ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
            d1 = ConvertDataGridViewToDataTable(dataGridView1);

            AggiornaGridView();
        }


        //tabcontrol pagina2
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        { //in base alla selezione cambio i valori della combobox successiva
            comboBox4.Visible = true;
            switch (comboBox3.SelectedIndex)
            {
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
                    comboBox4.Items.AddRange(DataTableToStringArray(Query("select clienti.email from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;")).Distinct().ToArray());

                    break;

                case 3:
                    comboBox4.Items.Clear();
                    comboBox4.Items.AddRange(DataTableToStringArray(Query("select data_ordine from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;")).Distinct().ToArray());

                    break;

                case 4:
                    comboBox4.Items.Clear();
                    comboBox4.Items.AddRange(DataTableToStringArray(Query("select oggetti.nome from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;")).Distinct().ToArray());

                    break;
            }
            AggiornaGridView();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        { //filtro per gli ordini
            string[] valori = new string[] { "id", "cliente_id", "data_ordine", "oggetto_id" };

            if (comboBox3.SelectedIndex - 1 == 0)
                d2 = Query($"select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id where ordini.{valori[comboBox3.SelectedIndex - 1]} = '{comboBox4.GetItemText(comboBox4.SelectedItem)}';");
            else if (comboBox3.SelectedIndex - 1 == 1 || comboBox3.SelectedIndex - 1 == 3)
                d2 = Query($"select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id where {valori[comboBox3.SelectedIndex - 1]} = '{comboBox4.GetItemText(comboBox4.SelectedItem)}';");
            else if (comboBox3.SelectedIndex - 1 == 2)
            {
                string date = comboBox4.Items[comboBox4.SelectedIndex].ToString();

                date = date.Split(' ')[0]; //modifico formato data
                date = date.Split('/')[2] + date.Split('/')[1] + date.Split('/')[0];

                d2 = Query($"select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id where ordini.{valori[comboBox3.SelectedIndex - 1]} = '{date}';");
            }
            AggiornaGridView();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string cliente = "";
            string oggetto = "";
            string data = "";

            bool doit = true;
            if (comboBox11.GetItemText(comboBox11.SelectedItem) != "")
            {
                cliente = comboBox11.GetItemText(comboBox11.SelectedItem).Split(' ')[0];
            }
            else
            {
                MessageBox.Show("Inserire un cliente valido");
                comboBox11.SelectedItem = "";
                doit = false;
            }
            if (comboBox12.GetItemText(comboBox12.SelectedItem) != "")
            {
                oggetto = comboBox12.GetItemText(comboBox12.SelectedItem).Split(' ')[0];
            }
            else
            {
                MessageBox.Show("Inserire un oggetto valido");
                comboBox12.SelectedItem = "";
                doit = false;
            }
            if (dateTimePicker1 != null)
            {
                DateTime temp = dateTimePicker1.Value;
                data = $"{temp.Year}/{temp.Month}/{temp.Day}";
            }
            else
            {
                MessageBox.Show("Inserire una data valida");
                dateTimePicker1.Value = DateTime.Now;
                doit = false;
            }

            if (doit)
            {
                ShortQuery($"insert into `ordini`(`cliente_id`, `data_ordine`, `oggetto_id`) values ({cliente},\"{data}\",{oggetto});");
                comboBox11.SelectedItem = "";
                comboBox12.SelectedItem = "";
                dateTimePicker1.Value = DateTime.Now;
            }

            d2 = Query("select ordini.id, email, ordini.data_ordine, oggetti.nome, costo from (clienti join ordini on clienti.id=ordini.cliente_id) join oggetti on oggetti.id=ordini.oggetto_id;");

            AggiornaGridView();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            comboBox10_SelectedIndexChanged(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Elimina();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            comboBox7_SelectedIndexChanged(sender, e);
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView2.Sort(dataGridView2.Columns[comboBox10.SelectedIndex], checkBox3.Checked ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
            d2 = ConvertDataGridViewToDataTable(dataGridView2);

            AggiornaGridView();
        }


        //tabcontrol pagina3
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

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e) {
            if (comboBox5.SelectedIndex == 3)
                d3 = Query($"select * from oggetti where oggetti.costo {comboBox7.GetItemText(comboBox7.SelectedItem)} {numericUpDown1.Value};");

            AggiornaGridView();
        }

        private void button6_Click(object sender, EventArgs e)
        { //aggiungi oggetto
            string nome = "";
            int costo = 0;

            bool doit = true;
            if (!String.IsNullOrWhiteSpace(textBox6.Text))
            {
                nome = textBox6.Text;
            }
            else
            {
                MessageBox.Show("Inserire un nome valido");
                textBox6.Text = "";
                doit = false;
            }
            if (numericUpDown2.Value > 0)
            {
                costo = (int)numericUpDown2.Value;
            }
            else
            {
                MessageBox.Show("Inserire un costo valido");
                numericUpDown2.Value = 10;
                doit = false;
            }

            if (doit)
            {
                ShortQuery($"insert into `oggetti`(`nome`, `costo`) values(\"{nome}\", \"{costo}\")");
                textBox6.Text = "";
                numericUpDown2.Value = 10;
            }

            d3 = Query("select * from oggetti;");

            AggiornaGridView();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Elimina();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox9_SelectedIndexChanged(sender, e);
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView3.Sort(dataGridView3.Columns[comboBox9.SelectedIndex], checkBox2.Checked ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
            d3 = ConvertDataGridViewToDataTable(dataGridView3);

            AggiornaGridView();
        }


        //funzioni
        public DataTable Query(string query)
        { //esegue le query
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

        public void ShortQuery(string query)
        {
            String ConnectionString = "server=localhost;uid=Utente1;pwd=password;database=ordini";
            MySqlConnection conn = new MySqlConnection(ConnectionString);

            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public string[] OrderArray(string[] values, bool decrescent)
        {
            if (decrescent)
            {
                Array.Sort(values);
                Array.Reverse(values);
            }
            else
                Array.Sort(values);

            return values;
        }

        public DataTable ConvertDataGridViewToDataTable(DataGridView dataGridView)
        {
            DataTable dataTable = new DataTable();

            // Aggiungi colonne alla DataTable usando i nomi delle colonne del DataGridView
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                dataTable.Columns.Add(column.Name, column.ValueType ?? typeof(string));
            }

            // Aggiungi righe alla DataTable usando i dati del DataGridView
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dataRow[cell.ColumnIndex] = cell.Value;
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        public string[] DataTableToStringArray(DataTable dt) //converte il risultato di una query in un array di stringhe
        {
            string[] result = new string[dt.Rows.Count];
            int index = 0;

            foreach (DataRow row in dt.Rows)
            {
                result[index] = string.Join(" ", row.ItemArray);
                index++;
            }

            return result;
        }

        private void Aggiorna() //riprende i valori direttamente dal database
        {
            String ConnectionString = "server=127.0.0.1;uid=Utente1;pwd=password;database=ordini";
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();

            AggiornaGridView();

            //clienti
            comboBox1.SelectedItem = comboBox1.Items[0];//azzero i valori
            comboBox2.Visible = false;
            comboBox2.Items.Clear();
            comboBox8.SelectedItem = comboBox8.Items[0];

            //ordini
            comboBox3.SelectedItem = comboBox3.Items[0];
            comboBox4.Visible = false;
            comboBox4.Items.Clear();
            comboBox10.SelectedItem = comboBox10.Items[0];
            comboBox11.Items.Clear();
            comboBox12.Items.Clear();
            comboBox11.Items.AddRange(DataTableToStringArray(Query("select clienti.id, clienti.email from clienti;")).Distinct().ToArray());
            comboBox12.Items.AddRange(DataTableToStringArray(Query("select oggetti.id, oggetti.nome from oggetti;")).Distinct().ToArray());


            //oggetti
            comboBox5.SelectedItem = comboBox5.Items[0];
            comboBox6.Items.Clear();
            comboBox6.Visible = false;
            comboBox7.Visible = false;
            numericUpDown1.Visible = false;
            comboBox9.SelectedItem = comboBox9.Items[0];

            conn.Close();
        }

        public void AggiornaGridView()
        {
            dataGridView1.DataSource = d1;
            dataGridView2.DataSource = d2;
            dataGridView3.DataSource = d3;
        }

        private void Elimina()
        {
            int selezionati = 0;
            string pk = "";
            string table = "";
            if (tabControl1.SelectedIndex == 0)
            {
                table = "clienti";
                selezionati = dataGridView1.SelectedRows.Count;
                if (selezionati > 0)
                    pk = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                else
                {
                    MessageBox.Show("Selezionare un elemento");
                    return;
                }
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                table = "ordini";
                selezionati = dataGridView2.SelectedRows.Count;
                if (selezionati > 0)
                    pk = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
                else
                {
                    MessageBox.Show("Selezionare un elemento");
                    return;
                }
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                table = "oggetti";
                selezionati = dataGridView3.SelectedRows.Count;
                if (selezionati > 0)
                    pk = dataGridView3.SelectedRows[0].Cells[0].Value.ToString();
                else
                {
                    MessageBox.Show("Selezionare un elemento");
                    return;
                }
            }

            var ris = MessageBox.Show("", "Conferma cancellazione", MessageBoxButtons.YesNo);
            try
            {
                if (ris == DialogResult.Yes)
                    ShortQuery($"delete from {table} where id='{pk}';");
            }
            catch
            {
                MessageBox.Show("L'elemento non può essere eliminato, visto che è associato ad un altro elemento");
            }
            AggiornaGridView();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}