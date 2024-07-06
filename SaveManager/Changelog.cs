using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaveManager
{
    public partial class Changelog : Form
    {
        public Changelog()
        {
            InitializeComponent();
        }

        private void Changelog_Load(object sender, EventArgs e)
        {
            textBox1.Text = new WebClient().DownloadString(Web.Web.DOMAIN_URL+"changelog.php");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
