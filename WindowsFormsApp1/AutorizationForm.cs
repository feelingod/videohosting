//using MySqlX.XDevAPI;
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
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace WindowsFormsApp1
{
    public partial class AutorizationForm : Form
    {
        DataBase dataBase = new DataBase();
        private string text = String.Empty;
        string name;
        int q = 0;
        int qq = 0;
        bool captcha;
        bool captcha_checked;
        Properties.Settings settings = new Properties.Settings();
        public AutorizationForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            checkBox1.Checked = Properties.Settings.Default.check;
            pictureBox2.BackColor = System.Drawing.Color.Transparent;
            checkBox1.BackColor = System.Drawing.Color.Transparent;
            if (checkBox1.Checked)
            {
                loginTB.Text = Properties.Settings.Default.login;
                passwordTB.Text = Properties.Settings.Default.password;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closeButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.Red;
        }

        private void closeButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.Black;  
        }
        Point lastPoint;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            passwordTB.UseSystemPasswordChar = false;
            pictureBox1.Image = Image.FromFile("otkrituy_glaz_zxc.png");
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            passwordTB.UseSystemPasswordChar = true;
            pictureBox1.Image = Image.FromFile("otkrituy_glaz111.png");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var loginUser = loginTB.Text;
            var passwordUser = passwordTB.Text;

            if (loginUser == "" || passwordUser== "")
            {
                MessageBox.Show("Заполните поля для ввода данных");
                return;
            }
            int user_id;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select user_name, user_password ,id from [User] where user_name = '{loginUser}' and user_password = '{passwordUser}'";
            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            this.name = loginUser;
            
            if (captcha)
            {
                if(textBox1.Text == this.text)
                {
                    captcha_checked = true;
                }
                else
                {
                    MessageBox.Show("Неверно введена капча..");
                    textBox1.Text = "";
                    pictureBox2.Image = this.CreateImage(pictureBox2.Width, pictureBox2.Height);
                    return;
                }
            }

            if (table.Rows.Count == 1) // если данные введены правильно
            {
                user_id = Convert.ToInt32(table.Rows[0].ItemArray[2]);
                if (captcha) // если пользователю нужно вести капчу
                {
                    if (captcha_checked) // капча введена правильно
                    {
                        if (checkBox1.Checked == true)
                            saveSettings();
                        else
                            reset_settings();

                        Form3 frm3 = new Form3(user_id);
                        this.Close();
                        frm3.Show();
                        greetinggg();
                    }

                    else // капча введена неверно
                        MessageBox.Show("Неверно введена капча...");
                }
                else //  если пользователю не должен был вводить капчу
                {
                    user_id = Convert.ToInt32(table.Rows[0].ItemArray[2]);
                    Form3 frm3 = new Form3(user_id);
                    this.Hide();
                    frm3.Show();
                    if (checkBox1.Checked == true)
                        saveSettings();
                    else
                        reset_settings();
                    greetinggg();
                }
            }
            else
            {
                if (qq < 2) // счётчик неправильных входов
                {
                    qq++;
                }
                else //перенос элементов доп. полей для входа по капче, если пользователь безуспешно попытался войти 3 раза
                {
                    this.Size = new Size(600, 550);
                    pictureBox2.Visible = true; textBox1.Visible = true; button2.Visible = true;
                    label4.Location = new Point(175,480);
                    ButtonLogin.Location = new Point(240, 440);
                    button1.Location = new Point(240,510);
                    pictureBox2.Image = this.CreateImage(pictureBox2.Width, pictureBox2.Height);
                    captcha = true;
                }
                q = 0; //старт 5-секундного таймера для блокировки кнопки входа
                timer1.Start();                
                ButtonLogin.Enabled = false;
            }
        }
        private void saveSettings() //запись и сохранение при отмеченной галочке (CheckBox.Checked = true) на флажке "Запомнить меня"
        {
            Properties.Settings.Default.check = checkBox1.Checked;
            Properties.Settings.Default.login = loginTB.Text;
            Properties.Settings.Default.password= passwordTB.Text;
            Properties.Settings.Default.Save();
        }
        private void reset_settings() //сброс логина и пароля при пустом флажке "Запомнить меня" (CheckBox.Checked = false)
        {
            Properties.Settings.Default.check = checkBox1.Checked;
            Properties.Settings.Default.login = "";
            Properties.Settings.Default.password = "";
            Properties.Settings.Default.Save();

        }
        private void greetinggg() // функция приветсвия
        {
            string greeting;
            DateTime currentTime = DateTime.Now;
            int currentHour = currentTime.Hour;

            if (currentHour >= 6 && currentHour < 12)
                greeting = "Доброе утро";
            else if (currentHour >= 12 && currentHour < 18)
                greeting = "Добрый день";
            else if (currentHour >= 18 && currentHour < 24)
                greeting = "Добрый вечер";
            else
                greeting = "Доброй ночи";
            MessageBox.Show($"{greeting}, {name}!");
        }

        private void button1_Click_1(object sender, EventArgs e) //функция перехода на форму регистрации
        {
            qq = 0;
            captcha = false;
            captcha_checked = false;
            Form2 newform = new Form2();
            newform.Show();
            this.Hide();
        }
        private void timer1_Tick(object sender, EventArgs e) //тик таймера, нужен для 5-секундного отсчёта до повторной попытки входа
        {
            if (q!=5)
            {
                label4.Visible = true;
                label4.Text = $"Неправильно введены данные! Подождите {5-q} секунд";
                q++;
            }
            else
            {
                timer1.Stop();
                label4.Visible=false;
                ButtonLogin.Enabled = true;
            }            
        }

        private void Form1_Load(object sender, EventArgs e) //настройка элементов для правильного отображения
        {
            captcha = false;
            this.Size = new Size(600, 400);
            ButtonLogin.Location = new Point(240, 280);
            label4.Location = new Point(170,327);
            button1.Location = new Point(240, 350);
            pictureBox2.Visible = false;
            textBox1.Visible = false;
            button2.Visible = false;
        }
        private Bitmap CreateImage(int Width, int Height) //функция создания CAPTCHA
        {
            captcha_checked= false;
            captcha = true;
            Random rnd = new Random();

            //создадаём изображение
            Bitmap result = new Bitmap(Width, Height);

            //вычисляем позицию текста
            int Xpos = rnd.Next(0, Width - 70);
            int Ypos = rnd.Next(15, Height - 15);

            Brush[] colors = { Brushes.Black,
                     Brushes.Red,
                     Brushes.RoyalBlue,
                     Brushes.Green };

            Graphics g = Graphics.FromImage((Image)result);

            g.Clear(Color.LightGray);

            text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; ++i) //отрисовка 4 случайных букв из списка выше
                text += ALF[rnd.Next(ALF.Length)];

            g.DrawString(text,
                         new Font("Arial", 15),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));

            g.DrawLine(Pens.Black,
                       new Point(0, 0),
                       new Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
                       new Point(0, Height - 1),
                       new Point(Width - 1, 0));

            ////создание белых точек
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);

            return result;
        }

        private void button2_Click(object sender, EventArgs e) // помещает созданную CAPTCH'у в picturebox.
        {
            pictureBox2.Image = this.CreateImage(pictureBox2.Width, pictureBox2.Height);

        }
    }
}
