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
using AxWMPLib;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApp1
{
   
    public partial class Form4 : Form
    {
        DataBase dataBase = new DataBase();
        int videoID;
        int userID;
        DateTime now;

        List<int> commentList = new List<int> { };

        public Form4(int video_id, int user_id)
        {
            InitializeComponent();

            this.userID = user_id;
            this.videoID = video_id;
            
            StartPosition = FormStartPosition.CenterScreen;
            
            titleLabel.BackColor = Color.Transparent;
            descriptionLabel.BackColor = Color.Transparent;
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;

            pictureBox2.Visible = false;

            loadViews(video_id);
            loadLikes(video_id);
            loadComments(video_id);
        }

        private void insertComments(int video_id)//загрузка комментариев в базу данных
        {
            now = DateTime.Now;
            string comment = commentTB.Text;
            string insertCommentQuery = $"insert into [Comments] (video_id, user_id, comment_date, comment_text) values ({videoID}, {userID}, '{now}', '{comment}')";
            using (SqlCommand command = new SqlCommand(insertCommentQuery, dataBase.getConnection()))
            {
                dataBase.openConnection();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                }
            }
        }

        private bool checkLike(int video_id, int user_id) //проверка на ваш лайк для данного видеоролика
        {
            string checkQuery = $"select user_id, video_id from [Likes] where video_id = {video_id} and user_id = {user_id}";
            using (SqlCommand command = new SqlCommand(checkQuery, dataBase.getConnection()))
            {
                dataBase.openConnection();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private void loadLikes(int video_id)
        {
            string searchQuery = $"select user_id from [Likes] where video_id = {video_id}";
            int lbLikes = 0;
            SqlCommand command = new SqlCommand(searchQuery, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lbLikes++;
            }
            label2.Text = Convert.ToString(lbLikes);
            reader.Close();
            dataBase.closeConnection();
            if (checkLike(videoID, userID))
            {
                pictureBox1.Visible = false;
                pictureBox2.Visible = true;
                //MessageBox.Show("Данному видео уже поставлен ваш лайк");
            }
            else
            {
                //MessageBox.Show("Данному видео вы ещё не поставили лайк");
            }
        }

        private void loadViews(int video_id) //загрузка просмотров
        {
            string uploadVideoQuery = $"select title, id, file_link, description from [Video] where id = {video_id}";
            string lbViews = "";
            SqlCommand command = new SqlCommand(uploadVideoQuery, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) // загрузка информации о видеоролике
            {
                string title = reader.GetString(0);
                string fileLink = reader.GetString(2);
                string description = reader.GetString(3);
                titleLabel.Text = title;
                descriptionLabel.Text = description;
                axWindowsMediaPlayer1.URL = fileLink;
            }
            reader.Close();
            dataBase.closeConnection();

            string searchQuery = $"select id, views_count from [Video] where id = {video_id}";
            SqlCommand searchVideo = new SqlCommand(searchQuery, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader reader2 = searchVideo.ExecuteReader();
            if (reader2.Read()) // если видео уже смотрели и оно занесено в базу данных, то прибавляется +1 просмотр
            {
                int views_count = reader2.GetInt32(1);
                reader2.Close();
                lbViews = Convert.ToString(views_count + 1);
                string updateQuery = $"update [Video] set views_count = {lbViews} where id = {video_id}";

                SqlCommand commandUpdateQuery = new SqlCommand(updateQuery, dataBase.getConnection());
                SqlDataReader reader3 = commandUpdateQuery.ExecuteReader();
                reader3.Read();
                reader3.Close();
            }
            dataBase.closeConnection();
            label1.Text = "Количество просмотров: " + lbViews;
        }
        
        private void button1_Click_1(object sender, EventArgs e) // Кнопка "Назад", переход к главной форме
        {
            Form3 form3 = new Form3(userID);
            axWindowsMediaPlayer1.URL = null;
            this.Close();
            form3.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e) // убирает лайк из БД
        {
            string deleteLikeQuery = $"delete from [Likes] where user_id = {userID} and video_id = {videoID}";
            using (SqlCommand command = new SqlCommand(deleteLikeQuery, dataBase.getConnection()))
            {
                dataBase.openConnection();
                using (SqlDataReader reader = command.ExecuteReader())
                    reader.Read();
            }
            pictureBox2.Visible = false;
            pictureBox1.Visible = true;
            int str = Convert.ToInt32(label2.Text);
            label2.Text = Convert.ToString(str - 1);
        }

        private void pictureBox1_Click(object sender, EventArgs e) // вставляет лайк в БД
        {
            string uploadQuery = $"insert into [Likes] (video_id, user_id) values ({videoID}, {userID})";
            SqlCommand commandUploadQuery = new SqlCommand(uploadQuery, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader reader = commandUploadQuery.ExecuteReader();
            reader.Read();
            reader.Close();
            dataBase.closeConnection();
            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
            int str = Convert.ToInt32(label2.Text);
            label2.Text = Convert.ToString(str + 1);
        }

        private void button2_Click(object sender, EventArgs e)///добавляет комментарий в базу данных и обновляет его на текущей форме
        {
            insertComments(videoID);
            loadComments(videoID);
            loadCommentsUserNames();

        }

        private void loadComments(int video_id) ///загрузка item'oв в listview для отображения комментариев
        {

            listView1.Clear();

            ImageList imageList = new ImageList();
            listView1.LargeImageList = imageList;
            listView1.LargeImageList.ImageSize = new Size(64, 64);

            listView1.TileSize = new Size(400, 100);
            listView1.ForeColor = Color.Black;
            listView1.BackColor = Color.LightGray;

            listView1.Columns.Add("UserName", 200);
            listView1.Columns.Add("CommentText", 300);

            string loadCommentsQuery = $"select user_id, comment_id, comment_date, comment_text from [Comments] where video_id = {video_id}";

            imageList.Images.Add($"image", Image.FromFile("C:\\Users\\Антон\\Desktop\\source\\repos\\WindowsFormsApp1\\WindowsFormsApp1\\Resources\\user_icon.png"));

            using (SqlCommand command = new SqlCommand(loadCommentsQuery, dataBase.getConnection()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int user_id = reader.GetInt32(0);
                        int comment_id = reader.GetInt32(1);
                        string comment_text = reader.GetString(3);

                        ListViewItem item = new ListViewItem();
                        item.ImageKey = "image";

                        commentList.Add(user_id);

                        item.Text = "";//сразу загрузить имена комментаторов нельзя, поэтому это сделано в функции loadCommentsUserNames() ниже
                        item.SubItems.Add(comment_text, ForeColor = Color.Black, BackColor = Color.LightGray, Font = new Font("Arial", 12, FontStyle.Regular));
                        listView1.Items.Add(item);
                        
                    }
                }
                loadCommentsUserNames();
            }
        }

        private void loadCommentsUserNames() // функция, загружающая имена пользователей, оставивших комментарии
        {
            string user_name = "";
            foreach (ListViewItem item in listView1.Items)
            {
                int user_id = commentList[item.Index];
                string loadCommetnSqlCommand = $"select user_name from [User] where id = {user_id}";
                using (SqlCommand loadCommentCommand = new SqlCommand(loadCommetnSqlCommand, dataBase.getConnection()))
                {
                    using (SqlDataReader reader = loadCommentCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user_name = reader.GetString(0);
                        }
                        else
                            user_name = "Unknown";
                    }
                }
                item.Text = user_name;
            }
        }
    }
}
