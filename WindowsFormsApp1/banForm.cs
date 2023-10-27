using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WindowsFormsApp1
{
    public partial class banForm : Form
    {
        DataBase dataBase = new DataBase();
        private int id;
        public banForm(int auth_id)
        {
            InitializeComponent();
            this.id = auth_id;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string sqlQuery = "UPDATE dbo.auth_table SET blocked = 1 WHERE auth_id=@id";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, dataBase.getConnection());
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
                this.Close();
            }
            catch (Exception )
            {
                MessageBox.Show("Ошибка блокировки аккаунта");
            }

        }
    }
}
