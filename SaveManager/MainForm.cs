using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaveManager
{
    public partial class MainForm : Form
    {
        
        RegistryUtility reg = new RegistryUtility();
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshList();
        }
        Dictionary<string,string> list = new Dictionary<string,string>();
        private void button2_Click(object sender, EventArgs e)
        {
            string temppatch = Path.GetTempFileName();
            reg.ExportRegistryKey("HKEY_CURRENT_USER\\Software\\Cheesecake Dev\\Streamer Life Simulator", temppatch);
            string output = System.IO.File.ReadAllText(temppatch);
            string encrypted = Convert.ToBase64String(Encoding.UTF8.GetBytes(output));
            System.IO.File.WriteAllText($"./savegames/{new Random().Next()}.slsg", $"{DateTime.Now}\n{encrypted}");
        }
        private void RefreshList()
        {
            if (!Directory.Exists("./savegames"))
                Directory.CreateDirectory("./savegames");
            foreach(var file in System.IO.Directory.GetFiles("./savegames"))
            {
                string savename = System.IO.File.ReadAllText(file).Split('\n')[0];
                list.Add(savename, file);
            }
            listBox1.Items.Clear();
            foreach(var item in list)
            {
                listBox1.Items.Add(item.Key);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var msg = MessageBox.Show("This will overwrite your current save, are you sure?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(msg == DialogResult.Yes)
            {
                
                string temppatch = Path.GetTempPath();
                MessageBox.Show(listBox1.SelectedItem.ToString());
                string enc = System.IO.File.ReadAllText(list[listBox1.SelectedItem.ToString()]);
                string dec = Encoding.UTF8.GetString(Convert.FromBase64String(enc.Split('\n')[1]));
                reg.ClearRegistryKey("Software\\Cheesecake Dev\\Streamer Life Simulator");
                System.IO.File.WriteAllText($"{temppatch}\\import.reg", dec);
                reg.ImportRegistryKey($"{temppatch}\\import.reg");
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var msg = MessageBox.Show("Do you want to delete your save game?","Warning",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (msg == DialogResult.Yes)
            {
                var msg2 = MessageBox.Show("Your save game will be deleted forever, do you want to continue?", "Last warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (msg2 == DialogResult.Yes)
                    reg.ClearRegistryKey("Software\\Cheesecake Dev\\Streamer Life Simulator");
                else
                    return;
            } else
            {
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var msg = MessageBox.Show("Do you want to delete your save game?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (msg == DialogResult.Yes)
            {
                var msg2 = MessageBox.Show("Your save game will be deleted forever, do you want to continue?", "Last warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (msg2 == DialogResult.Yes)
                    System.IO.File.Delete(list[listBox1.SelectedItem.ToString()]);
                else
                    return;
            }
            else
            {
                return;
            }
            RefreshList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Maybe in future?");
        }
    }
    public class RegistryUtility
    {
        public void ClearRegistryKey(string keyPath)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath, true))
            {
                if (key == null)
                {
                    Console.WriteLine($"Key {keyPath} not found.");
                    return;
                }

                foreach (var valueName in key.GetValueNames())
                {
                    key.DeleteValue(valueName);
                }

                foreach (var subKeyName in key.GetSubKeyNames())
                {
                    key.DeleteSubKeyTree(subKeyName);
                }
            }
        }
        public void ExportRegistryKey(string keyPath, string exportFilePath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("regedit.exe", $"/e \"{exportFilePath}\" \"{keyPath}\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    Console.WriteLine($"Registry key {keyPath} exported to {exportFilePath}.");
                }
                else
                {
                    Console.WriteLine($"Failed to export registry key {keyPath}.");
                }
            }
        }
        public void ImportRegistryKey(string importFilePath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("regedit.exe", $"/s \"{importFilePath}\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    Console.WriteLine($"Registry key imported from {importFilePath}.");
                }
                else
                {
                    Console.WriteLine($"Failed to import registry key from {importFilePath}.");
                }
            }
        }
    }
    
}
