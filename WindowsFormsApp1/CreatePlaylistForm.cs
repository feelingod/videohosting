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

namespace WindowsFormsApp1
{
    public partial class CreatePlaylistForm : Form
    {
        DataBase dataBase = new DataBase();
        private int userid;
        private string str = "";

        private string plName;
        public CreatePlaylistForm(int user_id)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            string qsg = $"SELECT SongName FROM Songs; select user_name from [User] where id={user_id}";
            SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            this.userid = user_id;
            while (reader.Read())
            {
                string songName = reader.GetString(0);
                listBox1.Items.Add(songName);
            }
            reader.Close();
            dataBase.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add(listBox1.SelectedItem);
            str += " ";
            str += listBox1.SelectedIndex.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                plName = textBox1.Text;

                string qsg = $"INSERT INTO [Playlists] (PlaylistName, songs_id, user_id) VALUES ('{plName}','{str}',{userid})";
                SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());
                dataBase.openConnection();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                reader.Close();
                dataBase.closeConnection();
                PGMainForm form = new PGMainForm(userid);
                this.Hide();
                form.Show();
            }
            else 
            {
                MessageBox.Show("Введите название плейлиста");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PGMainForm form = new PGMainForm(userid);
            this.Hide();
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.SelectedItems.Remove(listBox2.SelectedItem);
        }
    }
}
