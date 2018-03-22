using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sci_hubAutoLink.Popup
{
    public partial class Help : Form
    {
        Global glb = new Global();
        public Help()
        {
            InitializeComponent();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(glb.MyBlogLink);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(glb.MyGitHubLink);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
