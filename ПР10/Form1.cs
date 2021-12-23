using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace ПР10
{
    public partial class Form1 : Form
    {
        DataSet ds = new DataSet();
        SqlDataAdapter ad;
        SqlCommandBuilder commandBuilder;
        static string connectionString = @"Data Source=10.10.1.24;Initial Catalog=PR10 Maksim;User ID=pro-41;Password=Pro_41student";
        string sql = "SELECT * FROM Zakazi";
        SqlConnection connection = new SqlConnection(connectionString);
        public Form1()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // TODO: данная строка кода позволяет загрузить данные в таблицу "pR10_MaksimDataSet.Zakazi". При необходимости она может быть перемещена или удалена.
            this.zakaziTableAdapter.Fill(this.pR10_MaksimDataSet.Zakazi);
            connection.Open();
            ad = new SqlDataAdapter(sql, connection);

            ds = new DataSet();
            ad.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            // делаем недоступным столбец id для изменения
            dataGridView1.Columns["ID_zakaz"].ReadOnly = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["ID_zakazchik"].Index)
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 2; i++)
                {
                    if (Convert.ToInt32(dataGridView1[e.ColumnIndex, e.RowIndex].Value) == Convert.ToInt32(dataGridView1[e.ColumnIndex, i].Value))
                    {
                        for (int column = 0; column < dataGridView1.Columns.Count; column++)
                            dataGridView1[column, e.RowIndex].Value = dataGridView1[column, i].Value;
                        break;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables[0].NewRow(); // добавляем новую строку в DataTable
            ds.Tables[0].Rows.Add(row);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // удаляем выделенные строки из dataGridView1
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.Remove(row);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            ad.InsertCommand = new SqlCommand("Procedures", connection);
            ad = new SqlDataAdapter(sql, connection);
            commandBuilder = new SqlCommandBuilder(ad);
            ad.InsertCommand = new SqlCommand("Procedures", connection);
            ad.InsertCommand.CommandType = CommandType.StoredProcedure;
            ad.InsertCommand.Parameters.Add(new SqlParameter("@ID_Reklama", SqlDbType.Int, 0, "ID_reklama"));
            ad.InsertCommand.Parameters.Add(new SqlParameter("@ID_Reklamadate", SqlDbType.Int, 0, "ID_reklamadate"));
            ad.InsertCommand.Parameters.Add(new SqlParameter("@ID_manager", SqlDbType.Int, 0, "ID_Manager"));
            ad.InsertCommand.Parameters.Add(new SqlParameter("@ID_zakazchik", SqlDbType.Int, 0, "ID_zakazchik"));
            ad.InsertCommand.Parameters.Add(new SqlParameter("@datecomplete", SqlDbType.Date, 0, "Datecomplete"));
            ad.InsertCommand.Parameters.Add(new SqlParameter("@price", SqlDbType.Money, 0, "price"));
            SqlParameter parameter = ad.InsertCommand.Parameters.Add("@ID_zakaz", SqlDbType.Int, 0, "Id_zakaz");
            parameter.Direction = ParameterDirection.Output;
            ad.Update(ds);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
                for (int j = 0; j < dataGridView1.RowCount; j++)
                    if (dataGridView1[i, j].FormattedValue.ToString().Contains(textBox1.Text.Trim()))
                    {
                        dataGridView1.CurrentCell = dataGridView1[i, j];
                        if (i < dataGridView1.RowCount - 1)
                            dataGridView1[i, j].Style.BackColor = Color.AliceBlue;
                        dataGridView1[i, j].Style.ForeColor = Color.Blue;
                    }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ds = new DataSet();
            string str = textBox2.Text.ToString();
            string sqla = $"select * from Zakazi where price like '%{str}%'";
            ad = new SqlDataAdapter(sqla, connection);
            ad.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
