using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Feedback : System.Web.UI.Page
{
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Class = Request.QueryString["Class"] == null ? "" : Request.QueryString["Class"].ToString();
            string twenty;
    
            
     
            if(userInfo == null)
            {
                Response.Write("<script>alert('請先登入!'); location.href='Event.aspx'</script>");
            }
            else
            {
                getData();
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
    protected void getData()
    {

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String id = Convert.ToString(Request.QueryString["sno"]);
        aDict.Add("EventSNO", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"SELECT EventSNO, EventName ,SupportMeals  from Event  Where EventSNO=@EventSNO", aDict);
        lbl_EventName.Text = Convert.ToString(objDT.Rows[0]["EventName"]);
        bool Meals = Convert.ToBoolean(objDT.Rows[0]["SupportMeals"]);
        if (Meals)
        {
            P_Meals.Visible = true;
        }
        if (userInfo != null)
        {
            lbl_PName.Text = userInfo.UserName;
            lbl_PMail.Text = userInfo.UserMail;
            lbl_PPhone.Text = userInfo.UserPhone;
            lbl_PTel.Text = userInfo.UserTel;
        }
        string Class = (Request.QueryString["Class"] != null) ? Request.QueryString["Class"].ToString() : "";
        string Certificate = (Request.QueryString["Certificate"] != null) ? Request.QueryString["Certificate"].ToString() : "";
        
    }


    protected void btn_submit_Click(object sender, EventArgs e)
    {
        int Countsumbit = 0;
        Countsumbit++;
        //防止重複報名
        if (Countsumbit != 1)
        {
            return;
        }
        String errorMessage = "";
        int size = 20480000;
        for (int i = 1; i < 6; i++)
        {
            FileUpload fu = ((FileUpload)Master.FindControl("ContentPlaceHolder1").FindControl("fileup_Document" + i.ToString()));
            if (fu.HasFile)
            {
                if (fu.PostedFile.ContentLength > size) errorMessage += "檔案1不得大於20M\\n";
            }
        }
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        String id = Convert.ToString(Request.QueryString["sno"]);
        string EventDGroupNum = Event.GetEventDGroupNum(id);
        bool ChkEventDGroup = Event.ChkEventDGroupNum(userInfo.PersonSNO, EventDGroupNum);
        string System_ID = Event.ChkEventS22(id);
        if (!ChkEventDGroup && System_ID == "S00")
        {
            Response.Write("<script>alert('已報名相同課程!'); document.location.href='./Event.aspx'</script>");
            return;
        }
        if (!Event.ChkEventCount(id))
        {
            Response.Write("<script>alert('已超過報名上限!'); document.location.href='./Event.aspx'</script>");
            return;
        }
        string CBL_CertificateStutas = Request.QueryString["CBL_CertificateStutas"] != null ? Request.QueryString["CBL_CertificateStutas"].ToString() : "";
        string Class = (Request.QueryString["Class"] != null) ? Request.QueryString["Class"].ToString() : "";
        string Certificate = (Request.QueryString["Certificate"] != null) ? Request.QueryString["Certificate"].ToString() : "";
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();

        //上傳檔案
        if (!fileup_Document1.HasFile && !fileup_Document2.HasFile && !fileup_Document3.HasFile && !fileup_Document4.HasFile && !fileup_Document5.HasFile && !fileup_Document6.HasFile && Class == "" && Certificate == "")
        {
            aDict.Add("PersonSNO", userInfo.PersonSNO);
            aDict.Add("EventSNO", id);
            aDict.Add("NoticeType", "2");
            aDict.Add("Meals", rbl_Meals.SelectedValue);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            aDict.Add("EventGroupNum", EventDGroupNum);
            aDict.Add("EventInvite", 0);
            if (CBL_CertificateStutas != "")
            {
                aDict.Add("CertificateStutas", CBL_CertificateStutas);//正常報名時，取得他所勾選之 1:有初階 2:進階 3:初進階
            }
            else
            {
                aDict.Add("CertificateStutas", Certificate);//我有資格報名時，取得他所勾選之 1:有初階 2:進階 3:初進階
            }
            objDH.executeNonQuery("Insert Into EventD(EventSNO, PersonSNO, NoticeType, CertificateStutas,Meals,CreateUserID,EventGroupNum,EventInvite) Values(@EventSNO, @PersonSNO, @NoticeType, @CertificateStutas,@Meals,@CreateUserID,@EventGroupNum,@EventInvite)", aDict);
            Response.Write("<script>alert('報名成功!'); document.location.href='./PersonnelSite_Integral.aspx?Check=1'</script>");

        }
        else if (Class != "" && Certificate != "")
        {
            string Note = "";
            if (Class == "1" && Certificate == "1")
            {
                Note = "缺初階證書";
            }
            if (Class == "2" && Certificate == "1")
            {
                Note = "缺初階證書";
            }
            if (Class == "2" && Certificate == "2")
            {
                Note = "缺進階證書";
            }
            if (Class == "2" && Certificate == "3")
            {
                Note = "缺初進階證書";
            }
            //建立下載專區資料夾
            string folderName = "E" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string folderPath = Server.MapPath("../Download") + "/" + folderName + "/";
            Directory.CreateDirectory(folderPath);
            uploadFiles(folderPath);
            aDict.Add("DLOADNAME", userInfo.UserName + "審核文件");
            aDict.Add("DLOADURL", folderName);
            aDict.Add("ISENABLE", 1);
            aDict.Add("DLCSNO", 4);
            aDict.Add("CompletionClass1", Class);//1核心 2.專門
            aDict.Add("CompletionCertificateType", Certificate);//0 階未取得 1取得初階 2取得進階 3取得初進階
            aDict.Add("SYSTEM_ID", "S00");
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            string sql = @"Insert Into Download(DLOADNAME,DLOADURL,ISENABLE,DLCSNO,SYSTEM_ID,CreateUserID) 
                Values(@DLOADNAME, @DLOADURL,@ISENABLE,@DLCSNO,@SYSTEM_ID,@CreateUserID)  SELECT @@IDENTITY AS 'Identity'";
            DataTable dt = objDH.queryData(sql, aDict);
            string DLOADSNO = dt.Rows[0]["Identity"].ToString();
            aDict.Add("PersonSNO", userInfo.PersonSNO);
            aDict.Add("EventSNO", id);
            aDict.Add("NoticeType", "2");
            aDict.Add("DLOADSNO", DLOADSNO);
            aDict.Add("Note", Note);
            aDict.Add("Meals", rbl_Meals.SelectedValue);
            aDict.Add("EventInvite", 0);
            aDict.Add("EventGroupNum", EventDGroupNum);
            objDH.executeNonQuery("Insert Into EventD(EventSNO, PersonSNO, NoticeType,DLOADSNO,CreateUserID,CompletionClass1,CompletionCertificateType,Note,Meals,EventInvite,EventGroupNum) Values(@EventSNO, @PersonSNO, @NoticeType,@DLOADSNO,@CreateUserID ,@CompletionClass1,@CompletionCertificateType,@Note,@Meals,@EventInvite,@EventGroupNum)", aDict);
            Response.Write("<script>alert('報名成功!'); document.location.href='./PersonnelSite_Integral.aspx?Check=1'</script>");
        }
        else
        {
            //建立下載專區資料夾
            string folderName = "E" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string folderPath = Server.MapPath("../Download") + "/" + folderName + "/";
            Directory.CreateDirectory(folderPath);
            uploadFiles(folderPath);
            aDict.Add("DLOADNAME", userInfo.UserName + "審核文件");
            aDict.Add("DLOADURL", folderName);
            aDict.Add("ISENABLE", 1);
            aDict.Add("DLCSNO", 4);
            aDict.Add("SYSTEM_ID", "S00");
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            string sql = @"Insert Into Download(DLOADNAME,DLOADURL,ISENABLE,DLCSNO,SYSTEM_ID,CreateUserID) 
                Values(@DLOADNAME, @DLOADURL,@ISENABLE,@DLCSNO,@SYSTEM_ID,@CreateUserID)  SELECT @@IDENTITY AS 'Identity'";
            DataTable dt = objDH.queryData(sql, aDict);
            string DLOADSNO = dt.Rows[0]["Identity"].ToString();
            aDict.Add("PersonSNO", userInfo.PersonSNO);
            aDict.Add("EventSNO", id);
            aDict.Add("NoticeType", "2");
            aDict.Add("DLOADSNO", DLOADSNO);
            aDict.Add("Meals", rbl_Meals.SelectedValue);
            aDict.Add("EventInvite", 0);
            aDict.Add("EventGroupNum", EventDGroupNum);
            objDH.executeNonQuery("Insert Into EventD(EventSNO, PersonSNO, NoticeType,DLOADSNO,CreateUserID,Meals,EventInvite,EventGroupNum) Values(@EventSNO, @PersonSNO, @NoticeType,@DLOADSNO,@CreateUserID,@Meals,@EventInvite,@EventGroupNum)", aDict);
            Response.Write("<script>alert('報名成功!'); document.location.href='./PersonnelSite_Integral.aspx?Check=1'</script>");
        }


    }
    protected void uploadFiles(string folderPath)
    {
        for (int i = 1; i < 7; i++)
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
                if (fileInfo.Exists) fileInfo.Delete();
            }
        }
    }

}