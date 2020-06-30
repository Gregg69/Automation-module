using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Equipment
{
    public partial class Form1 : Form
    {
        OleDbConnection connection;
        string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=equipment.mdb;";
        OleDbDataAdapter adapter;
        OleDbCommandBuilder commandBuilder;
        DataSet ds;
        int iRow, prodCount;

        public Form1()
        {
            InitializeComponent();

            connection = new OleDbConnection(connectionString);
            connection.Open();
            prodCount = 0;
            iRow = 0;
            //--------------
            string query; OleDbCommand command; OleDbDataReader reader;

            query = "SELECT * FROM [Типы оборудования]";
            command = new OleDbCommand(query, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                // выводим данные столбцов текущей строки в listBox1
                comboBox2.Items.Add(reader[0].ToString() + " " + reader[1].ToString() + " ");
            }
            comboBox2.SelectedIndex = 0;

            query = "SELECT * FROM [Модели]";
            command = new OleDbCommand(query, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                // выводим данные столбцов текущей строки в listBox1
                comboBox1.Items.Add(reader[0].ToString() + " " + reader[1].ToString() + " ");
            }
            comboBox1.SelectedIndex = 0;

            query = "SELECT * FROM [Поставщики]";
            command = new OleDbCommand(query, connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                // выводим данные столбцов текущей строки в listBox1
                comboBox3.Items.Add(reader[0].ToString() + " " + reader[1].ToString() + " ");
            }
            comboBox3.SelectedIndex = 0;

            //--------------
            string sql = "SELECT * FROM [Оборудование]";
            adapter = new OleDbDataAdapter(sql, connection);
            ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataSource = ds.Tables[0];
            label9.Text = DateTime.Now.ToString();


        }

        // Новый паспорт
        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "0";
            textBox2.Text = "";
            textBox7.Text = "";
            textBox6.Text = "";
            label11.Text = "?";
        }

        // Обновить поля на форме
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            iRow = dataGridView1.CurrentRow.Index;
            label11.Text = dataGridView1.Rows[iRow].Cells[1].Value.ToString();
            comboBox2.SelectedIndex = (int)dataGridView1.Rows[iRow].Cells[8].Value-1;
            comboBox1.SelectedIndex = (int)dataGridView1.Rows[iRow].Cells[2].Value - 1;
            comboBox3.SelectedIndex = (int)dataGridView1.Rows[iRow].Cells[7].Value - 1;
            textBox3.Text = dataGridView1.Rows[iRow].Cells[1].Value.ToString();
            textBox4.Text = dataGridView1.Rows[iRow].Cells[3].Value.ToString();
            textBox5.Text = "0"; //dataGridView1.Rows[iRow].Cells[3].Value.ToString();
            textBox2.Text = dataGridView1.Rows[iRow].Cells[4].Value.ToString();
            textBox7.Text = dataGridView1.Rows[iRow].Cells[5].Value.ToString();
            textBox6.Text = dataGridView1.Rows[iRow].Cells[6].Value.ToString();
        }
        //double kol = double.Parse(textBox1.Text);


        // Закрыть форму
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            connection.Close();

        }


        // https://vscode.ru/prog-lessons/ms-access-i-c-sharp-rabotaem-s-bd.html
        // Запись в БД
        private void button4_Click(object sender, EventArgs e)
        {
        // Запись в таблицу Оборудование
            string t1 = textBox3.Text;  // ИнвН
            string t2 = (comboBox1.SelectedIndex + 1).ToString(); // ид модель
            string t3 = textBox4.Text; // серийный
            string t4 = textBox2.Text; // дата
            string t5 = textBox7.Text;  // цена
            string t6 = textBox6.Text;  // дата списания
            string t7 = (comboBox3.SelectedIndex + 1).ToString(); ;  // ид поставщик
            string t8 = (comboBox2.SelectedIndex + 1).ToString(); ;  // ид тип
            string query = "INSERT INTO [Оборудование] ([ИнвN], [Id_модель], [СерийныйN], [Дата], [Цена], [Дата списания], [id_поставщик], [id_тип]) " +
            "VALUES ("+
            "'" + t1+"',"+ 
            t2+", '"+
            t3 + "','" +
            t4 + "', " +
            t5 + ",'" +
            t6 + "'," +
            t7 + "," +
            t8 + 
            ")";

        // создаем объект OleDbCommand для выполнения запроса к БД MS Access
        OleDbCommand command = new OleDbCommand(query, connection);
        // выполняем запрос к MS Access
        command.ExecuteNonQuery();
        MessageBox.Show("Паспорт успешно сохранен в БД");


        // обновить таблицу на форме
        //dataGridView1.Cl   
        string sql = "SELECT * FROM [Оборудование]";
        adapter = new OleDbDataAdapter(sql, connection);
        ds = new DataSet();
        adapter.Fill(ds);
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.DataSource = ds.Tables[0];
        label9.Text = DateTime.Now.ToString();

            /*    

            // Получить код
            query = "SELECT [Код_заказ] FROM [Заказы]WHERE [Дата_создания]= "+
                "'" + t4 + "'";
            //query = "SELECT * FROM [Заказы]";// WHERE [Дата_создания]= "+
            //command = new OleDbCommand(query, connection);
            command.CommandText = query;
            object o = command.ExecuteScalar();
            string kod_zak = o.ToString();
       
                // Запись таблицы 
            //object o;
            string v1, v2, v3, v4;
            for (int i = 0; i < dataGridView1.Rows.Count-1; ++i)
            {
                //o = dataGridView2[j, i].Value;
                v1 = kod_zak;
                v2 = dataGridView1[1, i].Value.ToString();
                v3 = dataGridView1[4, i].Value.ToString();
                v4 = dataGridView1[6, i].Value.ToString();

                query = "INSERT INTO [Комплекты] ([Код_заказа], [Код_материала], [Количество], [Стоимость]) " +
                       "VALUES (" +
                       v1 + "," +
                       v2 + "," +
                       v3 + "," +
                       v4 +
                       ")";
                command = new OleDbCommand(query, connection);
                command.ExecuteNonQuery();
                MessageBox.Show("Заказ успешно сохранен в БД");

            };
            */




        }

           // печать таблицы
        private void button3_Click_1(object sender, EventArgs e)
        {
            printDocument1.Print();

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);
            dataGridView1.DrawToBitmap(bm, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));
            e.Graphics.DrawImage(bm, 0, 0);

        }

        // печать формы
        private void button2_Click(object sender, EventArgs e)
        {
            // https://docs.microsoft.com/ru-ru/dotnet/framework/winforms/advanced/how-to-print-a-windows-form?redirectedfrom=MSDN
            dataGridView1.Visible = false;
            CaptureScreen(); 
            printDocument2.Print();
            dataGridView1.Visible = true;
        }


        Bitmap memoryImage;

        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
        }

        private void printDocument2_PrintPage(System.Object sender,
               System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }


    }
}
