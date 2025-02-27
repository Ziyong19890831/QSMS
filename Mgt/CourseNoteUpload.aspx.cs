using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_CourseNoteUpload : System.Web.UI.Page
{
    UserInfo userInfo = null;
    DateTime NowTime = Convert.ToDateTime(DateTime.Now);
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];

            GetData();

        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string errorMessage = "";
        //標題
        if (string.IsNullOrEmpty(fileup_New.FileName)) errorMessage += "檔名不得為空\\n";


        string extension = Path.GetExtension(fileup_New.FileName).ToLowerInvariant();
        // 判斷是否為允許上傳的檔案附檔名
        List<string> allowedExtextsion = new List<string> { ".pdf" };
        if (allowedExtextsion.IndexOf(extension) == -1) errorMessage += "請上傳PDF類型檔案\\n";
        
        if (fileup_New.FileName.Length > 50) errorMessage += "檔案名稱不可大於50個字\\n";

        if (fileup_New.HasFile)
        {
            int size = fileup_New.PostedFile.ContentLength;
            //如果大於30M就跳訊息
            if (size > 30720000)
            {
                errorMessage += "檔案不得大於30M\\n";
            }
            else
            {
                //刪除前一個檔案
                if (File.Exists(Server.MapPath("../CourseNoteFile") + "/" + lbl_CourseFile.Text))
                    File.Delete(Server.MapPath("../CourseNoteFile") + "/" + lbl_CourseFile.Text);
                //新增上傳檔案
                fileup_New.SaveAs(Server.MapPath("../CourseNoteFile") + "/" + fileup_New.FileName);
            }
        }


        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        if (fileup_New.HasFile == true)
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            DataHelper objDH = new DataHelper();
            aDict.Add("RoleSNO", Request.QueryString["sno"]);
            aDict.Add("CourseFile", fileup_New.FileName);
            objDH.executeNonQuery("UPDATE Role SET CourseFile =@CourseFile WHERE RoleSNO =@RoleSNO", aDict);
        }

        Response.Write("<script>alert('修改成功!');window.opener.location.reload();;window.close(); </script>");

    }


    protected void GetData()
    {
        string id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataHelper objDH = new DataHelper();

        DataTable objDT = objDH.queryData("SELECT A.RoleSNO , A.RoleName , A.CourseFile from Role A WHERE A.RoleSNO = @sno", aDict);


        if (objDT.Rows.Count > 0)
        {
            lbl_CourseFile.Text = objDT.Rows[0]["CourseFile"].ToString();
            lbl_RoleName.Text = objDT.Rows[0]["RoleName"].ToString();
        }
    }

    private void getfiles()
    {

    }
}