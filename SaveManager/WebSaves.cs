using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json.Linq;
namespace SaveManager
{
    public partial class WebSaves : Form
    {
        public bool bNeedsRefresh = false;
        List<Save> saves1 = new List<Save>();
        UploadSave us = new UploadSave();
        public WebSaves()
        {
            InitializeComponent();
            saves1 = FetchSaves();
        }
        private List<Save> FetchSaves()
        {
            JArray j = JArray.Parse(new WebClient().DownloadString($"{Web.Web.DOMAIN_URL}getsaves.php"));
            List<Save> saves = new List<Save>();
            foreach (var item in j)
            {
                var save = new Save();
                save.author = (string)item["author"];
                save.name = (string)item["name"];
                save.content = (string)item["content"];
                save.createdate = (string)item["createdate"];
                saves.Add(save);
                SavesList.Items.Add(save.name);
            }
            return saves;
        }
        private void WebSaves_Load(object sender, EventArgs e)
        {
            
        }

        private void SavesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Save sv = saves1.Find(x=>x.name == SavesList.SelectedItem.ToString());
            AuthorLabel.Text = $"Author: {sv.author}";
            NameLabel.Text = $"Title: {sv.name}";
            DateLabel.Text = $"Create Date: {sv.createdate}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save sv = saves1.Find(x => x.name == SavesList.SelectedItem.ToString());
            
            if(sv.content != null)
            {
                if (System.IO.File.Exists($"./savegames/{sv.name}.slsg"))
                {
                    var msg = MessageBox.Show("Save game with same name already exists, overwrite?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (msg != DialogResult.Yes)
                    {
                        return;
                    }
                    System.IO.File.Delete($"./savegames/{sv.name}.slsg");
                }
                System.IO.File.WriteAllText($"./savegames/{sv.name}.slsg", $@"{sv.name}
{sv.content}");
                bNeedsRefresh = true;
                MessageBox.Show("Successfuly added save game to library","Notify",MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else
            {
                MessageBox.Show("Save is not seleceted", "Select save first",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            us = new UploadSave();
            us.Show();
        }
    }
    class Save
    {
        public string author, name, content, createdate;
        public Save() { }
    }
}
