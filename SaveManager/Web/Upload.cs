using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SaveManager.Web;
namespace SaveManager.Web
{
    internal class Upload
    {
        public static void UploadSave(Save sv)
        {
            new WebClient().DownloadString(Web.DOMAIN_URL + $"uploadsave.php?creator={sv.author}&name={sv.name}&content={sv.content}&createdate={sv.createdate}");
        }
    }
}
