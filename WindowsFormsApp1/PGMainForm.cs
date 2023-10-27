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
    public partial class PGMainForm : Form
    {
        DataBase dataBase = new DataBase();
        private int userid;

        public PGMainForm(int user_id)
        {
            InitializeComponent();
            this.userid = user_id;
            label3.Text = user_id.ToString();
            StartPosition = FormStartPosition.CenterScreen;
            string qsg = $"select user_name from [User] where id={user_id}";
            SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            reader.Close();
            dataBase.closeConnection();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            CreatePlaylistForm form = new CreatePlaylistForm(userid);
            this.Hide();
            form.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MyPlaylistsForm form = new MyPlaylistsForm(userid);
            this.Hide();
            form.Show();
        }
    }
}
