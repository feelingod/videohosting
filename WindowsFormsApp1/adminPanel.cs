﻿using System;
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
    public partial class adminPanel : Form
    {
        DataBase database = new DataBase();
        public adminPanel()
        {
            InitializeComponent();
            StartPosition= FormStartPosition.CenterScreen;

        }

    }
}
