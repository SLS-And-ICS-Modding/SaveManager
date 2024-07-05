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
    public partial class UploadSave : Form
    {
        public UploadSave()
        {
            InitializeComponent();
            RefreshList();
        }
        Dictionary<string, string> list = new Dictionary<string, string>();
        private void RefreshList()
        {
            if (!Directory.Exists("./savegames"))
                Directory.CreateDirectory("./savegames");
            list.Clear();
            foreach (var file in System.IO.Directory.GetFiles("./savegames"))
            {
                string savename = System.IO.File.ReadAllText(file).Split('\n')[0];
                list.Add(savename, file);
                comboBox1.Items.Add(savename);
            }
            
        }
        string GetPath(string savename)
        {

            foreach (var file in list)
            {
                if(file.Key ==  savename)
                    return file.Value;
            }
            return "";
        }

        private void UploadSave_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save sv = new Save();
            sv.author = textBox2.Text;
            sv.name = textBox1.Text;
            sv.createdate = DateTime.Now.ToString();
            sv.content = System.IO.File.ReadAllText(GetPath(comboBox1.Text)).Split('\n')[1]; // a bit crashable, but idc for now
            Web.Upload.UploadSave(sv);
            this.Close();
        }
    }
}
