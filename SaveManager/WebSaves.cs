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
        List<Save> saves1 = new List<Save>();
        public WebSaves()
        {
            InitializeComponent();
            saves1 = FetchSaves();
        }
        private List<Save> FetchSaves()
        {
            JArray j = JArray.Parse(new WebClient().DownloadString("http://localhost:8000/getsaves.php"));
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
    }
    class Save
    {
        public string author, name, content, createdate;
        public Save() { }
    }
}
