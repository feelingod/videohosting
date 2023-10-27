using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Upload_videos : Form
    {
        DataBase dataBase = new DataBase();
        int user_id;
        public Upload_videos(int user_id)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            label2.BackColor = System.Drawing.Color.Transparent;
            label3.BackColor = System.Drawing.Color.Transparent;
            label4.BackColor = System.Drawing.Color.Transparent;
            label5.BackColor = System.Drawing.Color.Transparent;
            this.user_id = user_id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string title = textBox2.Text;
            int user_id = this.user_id;
            string description = textBox3.Text;
            string file_link = textBox4.Text;
            string file_link_preview = textBox5.Text;
            DateTime now = DateTime.Now;
            string qsg = "INSERT INTO [Video] (title, description, upload_date, user_id, file_link, file_link_preview) VALUES (@title, @description, @upload_date, @user_id, @file_link, @file_link_preview)";            //string qsg = $"update [Video] set title = '{title}', description = '{description}', file_link = '{file_link}', file_link_preview = '{file_link_preview}' where id = {id}";
            //запись в БД с введёнными данными в соответствубщие textBox'ы
            SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());

            command.Parameters.AddWithValue("@title", title);
            command.Parameters.AddWithValue("@description", description);   
            command.Parameters.AddWithValue("@file_link", file_link);
            command.Parameters.AddWithValue("@file_link_preview", file_link_preview);
            command.Parameters.AddWithValue("@user_id", user_id);
            command.Parameters.AddWithValue("@upload_date", now); 
            dataBase.openConnection();

            int rowsAffected = command.ExecuteNonQuery();
            dataBase.closeConnection();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Видео добавлено");
            }
            else
            {
                MessageBox.Show("Не удалось добавить видео...");
            }
            dataBase.closeConnection();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(user_id);
            this.Close();
            form3.Show();
        }
    }
}
