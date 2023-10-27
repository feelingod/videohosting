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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class MyPlaylistsForm : Form
    {
        DataBase dataBase = new DataBase();
        private int user_id;
        int[] playlists_id = new int[10];
        public MyPlaylistsForm(int user_id)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            string qsg = $"SELECT [PlaylistName], id FROM Playlists WHERE user_id = {user_id};";
            SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            this.user_id = user_id;
            string plName = reader.GetString(0);
            listBox1.Items.Add(plName);
            int i = 0;
            while (reader.Read())
            {
                playlists_id[i] = reader.GetInt32(1);
                i++;
                plName = reader.GetString(0);
                listBox1.Items.Add(plName);
            }
            reader.Close();
            dataBase.closeConnection();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PGMainForm form = new PGMainForm(user_id);
            this.Hide();
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string qsg = $"SELECT [PlaylistName], [songs_id] FROM [Playlists] WHERE id = {playlists_id[listBox1.SelectedIndex]};";
            SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string plName = reader.GetString(0);
            label1.Text = plName;
            string plsongs = reader.GetString(1);
            reader.Close();
            string[] parts = plsongs.Split(' ').Skip(1).ToArray();
            string idList = string.Join(",", parts); // объединяем идентификаторы в строку с разделителями
            string qsg2 = $"SELECT SongName FROM Songs WHERE id IN ({idList});";
            SqlCommand command2 = new SqlCommand(qsg2, dataBase.getConnection());
            SqlDataReader reader2 = command2.ExecuteReader();
            listBox1.Items.Clear();
            while (reader2.Read()) // читаем все записи, пока они есть
            {
                string songName = reader2.GetString(0);
                listBox1.Items.Add(songName); // добавляем названия треков в ListBox1
            }
            reader2.Close();
            dataBase.closeConnection();
        }
    }
}
