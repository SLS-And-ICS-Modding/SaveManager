using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaveManager
{
    public partial class ExportSave : Form
    {
        RegistryUtility reg = new RegistryUtility();
        public ExportSave()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string temppatch = Path.GetTempFileName();
            reg.ExportRegistryKey("HKEY_CURRENT_USER\\Software\\Cheesecake Dev\\Streamer Life Simulator", temppatch);
            string output = System.IO.File.ReadAllText(temppatch);
            string encrypted = Convert.ToBase64String(Encoding.UTF8.GetBytes(output));
            System.IO.File.WriteAllText($"./savegames/{textBox1.Text}.slsg", $"{textBox1.Text}\n{encrypted}");
            MessageBox.Show("Successfully saved!","Notify",MessageBoxButtons.OK,MessageBoxIcon.Information);
            Globals.bReloadRequired = true;
            this.Close();
        }
    }
}
