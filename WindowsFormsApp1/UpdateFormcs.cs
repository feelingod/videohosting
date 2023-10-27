using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class UpdateFormcs : Form
    {
        DataBase dataBase = new DataBase();
        int user_id;
        public UpdateFormcs(int user_id, string lb1, string lb2, string lb3, string lb4, string lb5)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            textBox1.Text = lb1;
            textBox2.Text = lb2;
            textBox3.Text = lb3; //помещает переданные данные видео в textBox'ы
            textBox4.Text = lb4;
            textBox5.Text = lb5;
            this.user_id = user_id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string title = textBox2.Text;
                int id = Convert.ToInt32(textBox1.Text);
                string description = textBox3.Text;
                string file_link = textBox4.Text;
                string file_link_preview = textBox5.Text;  

                string qsg = "UPDATE [Video] SET title = @title, description = @description, file_link = @file_link, file_link_preview = @file_link_preview WHERE id = @id AND user_id = @user_id"; //обновляет данные в БД
                SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());

                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@file_link", file_link);
                command.Parameters.AddWithValue("@file_link_preview", file_link_preview);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@user_id", user_id);

                dataBase.openConnection();
                int rowsAffected = command.ExecuteNonQuery();
                dataBase.closeConnection();

                if (rowsAffected > 0)
                    MessageBox.Show("Данные успешно изменены");
                else
                    MessageBox.Show("Не удалось изменить данные");
                dataBase.closeConnection();
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }  
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(user_id);
            this.Hide();
            form3.ShowDialog();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)//меняет цвет
        {
            label2.BackColor = System.Drawing.Color.Transparent;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            label5.BackColor = System.Drawing.Color.Transparent;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            label4.BackColor = System.Drawing.Color.Transparent;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label3.BackColor = System.Drawing.Color.Transparent;
        }
    }
}
