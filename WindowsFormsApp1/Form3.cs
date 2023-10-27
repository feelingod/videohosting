using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlClient;
using System.Collections;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        DataBase dataBase = new DataBase();
        int user_id;
        string user_name;
        List<string> id_case = new List<string>(); // нужен для сбора всех id, которые сопоставляются с index'ом
                                                   // выбранного в listview item'а, т.е. видео, и переносится в форму плеера
        
        public Form3(int user_id) // выводит имя пользователя в правый верхний угол формы
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            string qsg = $"select user_name from [User] where id={user_id}";
            this.user_id = user_id;
            SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string name = "";
            name = reader.GetString(0);
            this.user_name = name;
            username.Text = name;
            reader.Close();
            dataBase.closeConnection();
        }

        private void textBox1_Enter(object sender, EventArgs e) // подсказка для ввода запроса в поле
        {
            if (textBox1.Text == "Введите запрос")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }
        
        private void textBox1_Leave(object sender, EventArgs e)//подсказка для ввода запроса в поле
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Введите запрос";
                textBox1.ForeColor = Color.Gray;
            }
        }
        // поиск видео в БД по введённому запросу с помозью функции LIKE в SQL
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            listView1.Clear(); //очистка item'ов в listview, чтобы предыдущие видео убрались
            id_case.Clear(); // очистка списка id'шников
            string query = textBox1.Text;
            string qsg = $"select title, description, file_link, file_link_preview, id from [Video] where title LIKE '%{query}%'";
            SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();

            ImageList imageList = new ImageList();
            listView1.LargeImageList = imageList;
            listView1.LargeImageList.ImageSize = new Size(256, 156);

            listView1.Columns.Add("Title", 500);
            listView1.Columns.Add("Description",600);
            
            int i = 0;
            try
            {
                // если такое видео нашлось, то выводятся превью с их названием и описанием
                while (reader.Read())
                {
                    i++;
                    string title = reader.GetString(0);
                    string description = reader.GetString(1);
                    string file_link_preview = reader.GetString(3);

                    string video_id = Convert.ToString(reader.GetInt32(4));
                    id_case.Add(video_id);

                    imageList.Images.Add($"image{i}", Image.FromFile(file_link_preview));
                    ListViewItem item = new ListViewItem();
                    item.ImageKey = $"image{i}";
                    item.Text = title;
                    item.Font = new Font("Courier new", 10, FontStyle.Bold);
                    item.SubItems.Add(description);
                    listView1.Items.Add(item);
                }
            }
            //если был пустой запрос
            catch
            {
                listView1.Clear();
                id_case.Clear(); //очистка списка с id, чтобы не 
            }
            
            reader.Close();
            dataBase.closeConnection();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e) //переход на форму плеера по индексу нажатого видео
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int selectedIndex = listView1.SelectedItems[0].Index;
                int id = Convert.ToInt32(id_case[selectedIndex]);

                string qsg = $"select id, title, description, file_link, file_link_preview from [Video] where id = {id}";

                SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());
                dataBase.openConnection();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                int video_id = reader.GetInt32(0);
                this.Hide();
                Form4 form4 = new Form4(video_id, user_id);
                form4.ShowDialog();
                reader.Close();
                dataBase.closeConnection();
            }
        }


        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pictureBox1_Click(sender, e);
            }
        }

        private void мойПрофильToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Profile_Form form = new Profile_Form(user_id, user_name);
            this.Close();
            form.Show();
        }


        private void выйтиИзАккаунтаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutorizationForm frm = new AutorizationForm();
            this.Close();
            frm.Show();
        }

        private void добавитьВидеоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Upload_videos upload_Videos = new Upload_videos(user_id);
            this.Close();
            upload_Videos.Show();
        }
    }
}
