using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Reflection.Emit;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        DataBase dataBase = new DataBase();
        public Form2()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            label1.BackColor = System.Drawing.Color.Transparent;
            closeButton.BackColor = System.Drawing.Color.Transparent;
            label2.BackColor = System.Drawing.Color.Transparent;
            label3.BackColor = System.Drawing.Color.Transparent;
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            label5.BackColor = System.Drawing.Color.Transparent;
        }

        private void closeButton_Click_1(object sender, EventArgs e)
        {
            this.Close();
            AutorizationForm frm1 = new AutorizationForm();
            frm1.ShowDialog();
        }

        private void closeButton_MouseEnter_1(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.Red;
        }

        private void closeButton_MouseLeave_1(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.Black;
        }
        Point lastPoint;

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void label1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void label1_MouseDown_1(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            passwordTB.UseSystemPasswordChar = false;
            pictureBox1.Image = Image.FromFile("otkrituy_glaz_zxc.png");
        }

        private void pictureBox1_MouseUp_1(object sender, MouseEventArgs e)
        {
            passwordTB.UseSystemPasswordChar = true;
            pictureBox1.Image = Image.FromFile("otkrituy_glaz111.png");
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (checkUser()) // если пользователь с таким же логином существует, то не создаётся аккаунт
            {
                return;
            }
            var loginUser = loginTB.Text;
            var passwordUser = passwordTB.Text;
            var emailUser = emailTB.Text;
            DateTime now = DateTime.Now;
            DataTable table = new DataTable();
            string querystring = $"insert into [User] (user_name, user_password, email, registration_date) values ('{loginUser}','{passwordUser}', '{emailUser}', '{now}')";
            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            dataBase.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан", "Крутяк!");
                AutorizationForm frm1 = new AutorizationForm();
                this.Hide();
                frm1.ShowDialog();

            }

        }
        private Boolean checkUser()
        {
            var loginUser = loginTB.Text;
            var passwordUser = passwordTB.Text;

            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();

            string querystring = $"select user_name from [User] where user_name = '{loginUser}'"; // поиск по логину

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)//если нашёлся такой логин в БД
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
                return false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            AutorizationForm frm1 = new AutorizationForm();
            this.Hide();
            frm1.ShowDialog();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
