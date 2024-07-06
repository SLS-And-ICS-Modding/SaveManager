using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SaveManager.Web;
namespace SaveManager.Web
{
    internal class Update
    {
        public static void UpdateAvailablePopup()
        {
            var msgbox = MessageBox.Show("An newer version was released\nDo you want to update?", "New Update!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (msgbox == DialogResult.Yes) {
                Process.Start("https://github.com/SLS-And-ICS-Modding/SaveManager/releases");
            }
        }
        public static void CheckUpdate()
        {
            using (WebClient client = new WebClient())
            {
                if (client.DownloadString($"{Web.DOMAIN_URL}checkupdate.php?build={Globals.sBuild}") != "0")
                    UpdateAvailablePopup();
            }
        }
    }
}
