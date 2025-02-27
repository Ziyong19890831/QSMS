using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_FileBrower : System.Web.UI.Page
{
    UserInfo userInfo = null;
    public string MyImages = "";
    public string MyFiles = "";
    string wPath = "";
    string rPath = "";
    public string Manage = "";


    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
        wPath = "../Upload/" + userInfo.PersonSNO + "/";
        rPath = Server.MapPath("..") + "\\Upload\\" + userInfo.PersonSNO;

        //type=Files&CKEditor=editor1&CKEditorFuncNum=1&langCode=zh
        Manage = Request.QueryString["type"];

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //GetAllFiles();
        }
        GetAllFiles();
    }

    string[] ImageExt = { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
    protected void GetAllFiles()
    {

        string list_files = "";
        string list_images = "";


        if (Directory.Exists(rPath) == true) {
            string[] files = Directory.GetFiles(rPath);
            int fcount = 0;
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fi = new FileInfo(files[i]);
                string chkID = "f" + i.ToString();
                string chk = "<input id='" + chkID + "' name='files' type='checkbox' value='" + fi.Name + "' />";
                if (ImageExt.Any(s => fi.Extension.ToLower().Contains(s)))
                {
                    //選取模式
                    if (!string.IsNullOrEmpty(Manage))
                    {
                        list_images += "<div class='fbox content_box'>";
                        list_images += "    <div class='tlt'>" + fi.Name + "</div>";
                        list_images += "    <a class='inbox' href=\"javascript:select('" + wPath + fi.Name + "')\">";
                        list_images += "        <div class='imgblock'><img src='" + wPath + fi.Name + "' /></div>";
                        list_images += "    </a>";
                        list_images += "    <div class='footerbox'>"+ chk;
                        list_images += "    </div>";
                        list_images += "</div>";
                    }
                    //管理模式
                    else
                    {
                        list_images += "<div class='fbox content_box'>";
                        list_images += "    <div class='tlt'>" + fi.Name + "</div>";
                        list_images += "    <div class='imgblock'><img src='" + wPath + fi.Name + "' /></div>";
                        list_images += "    <div class='footerbox'>" + chk;
                        list_images += "    </div>";
                        list_images += "</div>";
                    }
                }
                else
                {
                    if (fcount % 4 == 0) list_files += "<tr>";

                    //選取模式
                    if (!string.IsNullOrEmpty(Manage))
                    {
                        list_files += "<td>" + chk + "<a href=\"javascript:select('" + wPath + fi.Name + "')\">" + fi.Name + "</a></td>";
                    }
                    //管理模式
                    else
                    {
                        list_files += "<td>" + chk + fi.Name + "</td>";
                    }

                    if (fcount % 4 == 3) list_files += "</tr>";
                    fcount += 1;
                }
            }
            MyImages = list_images;
            MyFiles = list_files;
        }

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string errorMessage = "";
        int size = fileup_New.PostedFile.ContentLength;
        if (size > 30720000) errorMessage += "檔案不得大於30M\\n";

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        if (Directory.Exists(rPath) == false)
        {
            Directory.CreateDirectory(rPath);
        }

        if (fileup_New.HasFile)
        {
            fileup_New.SaveAs(rPath + "\\" + fileup_New.FileName);
            Utility.showMessage(Page, "msg", "上傳成功!");
            GetAllFiles();
        }
        else
        {
            Utility.showMessage(Page, "msg", "未選取檔案!");
        }

    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        string files = Request.Form["files"];
        if (!string.IsNullOrEmpty(files))
        {
            string[] deletefiles = files.Split(',');
            for(int i = 0; i < deletefiles.Length; i++) {
                File.Delete(rPath + "\\" + deletefiles[i]);
            }
            Utility.showMessage(Page, "msg", "刪除" + deletefiles.Length + "個檔案成功");
            GetAllFiles();
        }
        else {
            Utility.showMessage(Page, "msg", "未選取任何檔案!");
        }
    }


}