using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SaveManager.Web;
namespace SaveManager.Web
{
    internal class Upload
    {
        public static void UploadSave(Save sv)
        {
            using (WebClient client = new WebClient())
            {
                var data = new System.Collections.Specialized.NameValueCollection();
                data["creator"] = sv.author;
                data["name"] = sv.name;
                data["content"] = sv.content;
                data["createdate"] = sv.createdate;

                var response = client.UploadValues(Web.DOMAIN_URL + "uploadsave.php", "POST", data);
            }
        }
    }
}
