using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Upload_AE : System.Web.UI.Page
{
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            if (work.Equals("N"))
            {
                newData();
                ButtonOK.Text = "新增";
            }
            else
            {
                setClassCode(ddl_Download_Class, null);
                getData();
            }
        }
    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        String errorMessage = "";

        if (txt_Title.Text.Length > 50) errorMessage += "名稱字元過多！\\n";
        if (txt_Title.Text.Length == 0) errorMessage += "請輸入名稱！\\n";
        if (ddl_Download_Class.SelectedValue == "") errorMessage += "請選擇分類!\\n";

        int size = 20480000;
        for (int i = 1; i < 6; i++)
        {
            FileUpload fu = ((FileUpload)Master.FindControl("ContentPlaceHolder1").FindControl("fileup_Document" + i.ToString()));
            if (fu.HasFile)
            {
                if (fu.PostedFile.ContentLength > size) errorMessage += "檔案1不得大於20M\\n";
            }
        }


        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }


        if (Work.Value.Equals("NEW"))
        {

            DataHelper objDH = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();

            //建立下載專區資料夾
            string folderName = "Q" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string folderPath = Server.MapPath("../Download") + "/" + folderName + "/";
            Directory.CreateDirectory(folderPath);
            //上傳檔案
            uploadFiles(folderPath);

            aDict.Add("DLOADNAME", txt_Title.Text);
            aDict.Add("DLOADURL", folderName);
            aDict.Add("ISENABLE", 1);
            aDict.Add("DLCSNO", ddl_Download_Class.SelectedValue);
            if (ddl_OrderSeq.SelectedValue == "")
                aDict.Add("OrderSeq", DBNull.Value);
            else aDict.Add("OrderSeq", ddl_OrderSeq.SelectedValue);
            aDict.Add("SYSTEM_ID", "S00");
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            aDict.Add("DLOADNote", txt_Note.Text);
           
            string sql = @"Insert Into Download(DLOADNAME,DLOADURL,ISENABLE,[DLOADNote],DLCSNO,SYSTEM_ID,CreateUserID,OrderSeq) 
                Values(@DLOADNAME, @DLOADURL,@ISENABLE,@DLOADNote,@DLCSNO,@SYSTEM_ID,@CreateUserID,@OrderSeq)  SELECT @@IDENTITY AS 'Identity'";

            DataTable dt = objDH.queryData(sql, aDict);

            //寫入適用人員
            Utility.insertRoleBind(cb_Role, dt.Rows[0]["Identity"].ToString(), "Upload_AE", userInfo.PersonSNO);

            Response.Write("<script>alert('新增成功!');document.location.href='./Upload.aspx'; </script>");

        }
        else
        {

            DataHelper objDH = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            string folderName = txt_URL.Value;
            string folderPath = Server.MapPath("../Download") + "/" + folderName + "/";
            //上傳檔案
            uploadFiles(folderPath);
            aDict.Add("DLOADNote", txt_Note.Text);
            aDict.Add("DLCSNO", ddl_Download_Class.SelectedValue);
            aDict.Add("DLOADSNO", txt_PID.Value);
            aDict.Add("DLOADNAME", txt_Title.Text);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            if (ddl_OrderSeq.SelectedValue == "")
            { 
                aDict.Add("OrderSeq", DBNull.Value);
            }
            else
            {
                aDict.Add("OrderSeq", ddl_OrderSeq.SelectedValue);
            }
                
            
            aDict.Add("SYSTEM_ID", "S00");
            
            string sql = @"
                update Download set DLOADNAME=@DLOADNAME, ModifyUserID=@ModifyUserID, ModifyDT=getdate(),DLOADNote=@DLOADNote,DLCSNO=@DLCSNO,
                    OrderSeq=@OrderSeq, SYSTEM_ID=@SYSTEM_ID where DLOADSNO=@DLOADSNO ";
            objDH.executeNonQuery(sql, aDict);

            //寫入適用人員
            Utility.insertRoleBind(cb_Role, txt_PID.Value, "Upload_AE", userInfo.PersonSNO);

            Response.Write("<script>alert('修改成功!');document.location.href='./Upload.aspx'; </script>");
           

        }

    }

    protected void newData()
    {
        Work.Value = "NEW";
        setClassCode(ddl_Download_Class, "---請選擇分類---");
        Utility.setRoleBind(cb_Role, "0", "");
    }

    protected void getData()
    {
        String No = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("DLOADSNO", No);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            select *
            from Download D
                LEFT JOIN SYSTEM S on D.SYSTEM_ID=S.SYSTEM_ID
                LEFT JOIN DownloadClass DC on DC.DLCSNO=D.DLCSNO
            Where DLOADSNO=@DLOADSNO
        ", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_PID.Value = objDT.Rows[0]["DLOADSNO"].ToString();
            txt_URL.Value = objDT.Rows[0]["DLOADURL"].ToString();
            txt_Title.Text = objDT.Rows[0]["DLOADNAME"].ToString();
            txt_Note.Text = objDT.Rows[0]["DLOADNote"].ToString();
            ddl_Download_Class.SelectedValue = objDT.Rows[0]["DLCSNO"].ToString();
            ddl_OrderSeq.SelectedValue = objDT.Rows[0]["OrderSeq"].ToString();
            Utility.setRoleBind(cb_Role, txt_PID.Value, "Upload_AE");
            getFolderFiles(txt_URL.Value);
        }
    }

    protected void setClassCode(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY DLCSNO) as ROW_NO, DLCSNO, DLCNAME  FROM DownloadClass Where ISENABLE>0", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }

    protected void getFolderFiles(string dirID)
    {
        string[] files = Directory.GetFiles(Server.MapPath("../Download") + "/" + dirID);
        for (int i = 0; i < 5; i++)
        {
            if (i < files.Length)
            {
                FileInfo fileInfo = new FileInfo(files[i]);
                ((Literal)Master.FindControl("ContentPlaceHolder1").FindControl("lt_file" + (i + 1).ToString())).Text = fileInfo.Name;
                ((FileUpload)Master.FindControl("ContentPlaceHolder1").FindControl("fileup_Document" + (i + 1).ToString())).Visible = false;
                ((Button)Master.FindControl("ContentPlaceHolder1").FindControl("btn_delfile" + (i + 1).ToString())).Visible = true;
            }
            else
            {
                ((Button)Master.FindControl("ContentPlaceHolder1").FindControl("btn_delfile" + (i + 1).ToString())).Visible = false;
            }
        }
    }

    protected void btn_delfile_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        String id = btn.CommandArgument;
        ((Literal)Master.FindControl("ContentPlaceHolder1").FindControl("lt_file" + id)).Visible = false;
        ((FileUpload)Master.FindControl("ContentPlaceHolder1").FindControl("fileup_Document" + id)).Visible = true;
        ((Button)Master.FindControl("ContentPlaceHolder1").FindControl("btn_delfile" + id)).Visible = false;
    }

    protected void uploadFiles(string folderPath)
    {
        for (int i = 1; i < 6; i++)
        {
            Literal lt = ((Literal)Master.FindControl("ContentPlaceHolder1").FindControl("lt_file" + i.ToString()));
            FileUpload fu = ((FileUpload)Master.FindControl("ContentPlaceHolder1").FindControl("fileup_Document" + i.ToString()));
            if (fu.HasFile)
            {
                fu.SaveAs(folderPath + fu.FileName);
            }
            if (lt.Visible == false)
            {
                FileInfo fileInfo = new FileInfo(folderPath + lt.Text);
                if(fileInfo.Exists) fileInfo.Delete();
            }
        }
    }

}