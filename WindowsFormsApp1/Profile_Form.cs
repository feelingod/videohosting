using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Profile_Form : Form
    {
        DataBase dataBase = new DataBase();
        int user_id;
        string name;
        List<string> id_case = new List<string>(); // нужен для операций "редактировать" и "удалить", при выборе определенного видео
        string title;
        string description;
        string file_link;
        string file_link_preview;
        string id;


        int listBoxCurrentIndex;
        public Profile_Form(int user_id, string user_name)//конструктор...
        {
            InitializeComponent();
            this.name = user_name;
            this.user_id = user_id;
            videoUpload();
        }
        private void videoUpload ()//загружает все видео, принадлежащие текущему пользователю.
        {
        listView1.Clear();
            
        string qsg = $"select title, description, file_link, file_link_preview, id from [Video] where user_id = {user_id}";
        SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());
        dataBase.openConnection();
        SqlDataReader reader = command.ExecuteReader();
        ImageList imageList = new ImageList();
        listView1.LargeImageList = imageList;
        listView1.LargeImageList.ImageSize = new Size(150, 90);//настройка размера превью видео
        listView1.Columns.Add("title", 1000);

            int i = 0;
            try// добавляет видео с их названием и превью
            {
                while (reader.Read())
                {
                    i++;
                    string title = reader.GetString(0);
                    string description = reader.GetString(1);
                    string file_link = reader.GetString(2);
                    string file_link_preview = reader.GetString(3);
                    string video_id = Convert.ToString(reader.GetInt32(4));
                    id_case.Add(video_id);

                    imageList.Images.Add($"image{i}", Image.FromFile(file_link_preview));
                    ListViewItem item = new ListViewItem();

                    item.ImageKey = $"image{i}";
                    item.Text = title;
                    item.Font = new Font("Courier new", 14);
                    listView1.Items.Add(item);
                }
}
            catch (Exception eeee)
                {
                MessageBox.Show(eeee.Message); 
                }
            finally //передает в один textbox кол-во видео пользователя, а в другой его имя
            {
                quantityVideos.Text = i.ToString();
                nameLB.Text = this.name;
                reader.Close();
                dataBase.closeConnection();
            }
        }
                


        private void button1_Click(object sender, EventArgs e)// кнопка "Назад".. переход к главной форме поиска
        {
            Form3 form = new Form3(user_id);
            this.Close();
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e) // переход к форме добавления видео
        {
            Upload_videos upload_Videos = new Upload_videos(user_id);
            this.Close();
            upload_Videos.Show();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e) //сообщение о подтверждении удаления видео
        {
            DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите удалить это видео?",
                "Внимание", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes) // если пользователь  нажал "Да"
            {
                int id = Convert.ToInt32(id_case[this.listBoxCurrentIndex]);

                string qsg = $"delete from [Video] where id = {id} and user_id = {user_id}";
                SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());
                dataBase.openConnection();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                reader.Close();
                dataBase.closeConnection();
                videoUpload();

            }
            //если он нажал "нет", то ничего не будет
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)//запоминался id выбранного видео для функций подменю (редактировать, удалить)
        {
            try
            {
                int selectedIndex = listView1.SelectedItems[0].Index;
                this.listBoxCurrentIndex = selectedIndex;

            }
            catch { }
            
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)//переход в форму редактирования видео с передачей в него данных выбранного
        {
            int id = Convert.ToInt32(id_case[this.listBoxCurrentIndex]);
            string qsg = $"select id, title, description, file_link, file_link_preview from [Video] where id = {id}";

            SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            int video_id = reader.GetInt32(0);
            this.id = video_id.ToString();
            this.title = reader.GetString(1);
            this.description = reader.GetString(2);
            this.file_link = reader.GetString(3);
            this.file_link_preview = reader.GetString(4);

            this.Close();
            UpdateFormcs form = new UpdateFormcs(user_id, this.id, title, description, file_link, file_link_preview);
            form.Show();
            reader.Close();
            dataBase.closeConnection();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)//настраивает позицию подменю так, чтобы оно появилось под курсором мыши
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this, new Point(e.Location.X, e.Location.Y + panel1.Width));
            }
        }
    }
}
