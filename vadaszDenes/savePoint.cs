using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace vadaszDenes
{
    public partial class savePoint : UserControl
    {
        public savePoint()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            //Combine the name and the points into one line and append it int the leaderboard.txt file.
            string sor ="\n"+ name.Text + ";" + pointLabel.Text.Split(':')[1].Trim();
            File.AppendAllText("leaderboard.txt", sor);
            Application.Exit();
        }
    }
}
