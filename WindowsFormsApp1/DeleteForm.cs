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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class DeleteForm : Form
    {
        DataBase dataBase = new DataBase();
        int user_id;
        

        public DeleteForm(int userid)
        {
            InitializeComponent();
            this.user_id = userid;
            label1.BackColor = System.Drawing.Color.Transparent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string id = textBox1.Text;

                string qsg = $"delete from [Video] where id = {id} and user_id = {user_id}";
                SqlCommand command = new SqlCommand(qsg, dataBase.getConnection());
                dataBase.openConnection();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                reader.Close();
                dataBase.closeConnection();
            }
            catch
            {
                MessageBox.Show("Не удалось удалить данные\rВы можете удалять только свои видео!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3(user_id);
            this.Hide();
            frm.Show();
        }
    }
}
