using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_FileUpload : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;
    string wPath = "";
    string rPath = "";
    string AuditPath = "";

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            setCoursePlanningClassHasCtype3(ddl_CoursePlanningClass, "請選擇");
            SetCourse(ddl_CourseName, "請選擇");

        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string errorMessage = "";
        int size = file_Upload.PostedFile.ContentLength;
        if (size > 30720000) errorMessage += "檔案不得大於30M\\n";
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }
        if (string.IsNullOrEmpty(ddl_CoursePlanningClass.SelectedItem.Value))
        {
            errorMessage += "請選擇欲上傳的課程規劃";
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }
        if (string.IsNullOrEmpty(ddl_CourseName.SelectedItem.Value))
        {
            errorMessage += "請選擇欲上傳的課程";
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }
        string Audit = CheckLastCourse(ddl_CourseName.SelectedValue, userInfo.PersonSNO);
        if (Audit == "" || Audit == "2")
        {


            if ((file_Upload != null) && (file_Upload.PostedFile.ContentLength > 0) && !string.IsNullOrEmpty(file_Upload.FileName))
            {
                string extension = Path.GetExtension(file_Upload.FileName).ToLowerInvariant();
                List<string> allowedExtextsion = new List<string> { ".zip", ".pdf" };
                if (allowedExtextsion.IndexOf(extension) == -1) errorMessage += "請上傳 (zip)或(pdf) 類型檔案";
                if (!String.IsNullOrEmpty(errorMessage))
                {
                    Utility.showMessage(Page, "ErrorMessage", errorMessage);
                    return;
                }
                //開始上傳
                wPath = "../Upload/" + userInfo.PersonSNO + "/";
                rPath = Server.MapPath("..") + "\\Upload\\" + userInfo.PersonSNO;
                /// Mgt / UploadAudit_AE.aspx ? Psno = 106142 & Csno = 54
                string doamin= System.Environment.UserDomainName; 
                AuditPath = "http://pc.pjm.iisigroup.com/QSMS/Mgt/UploadAudit_AE.aspx?Psno=" + userInfo.PersonSNO+ "&Csno="+ddl_CourseName.SelectedValue;
                if (Directory.Exists(rPath) == false)//建立PersonSNO資料夾
                {
                    Directory.CreateDirectory(rPath);
                }
                if (file_Upload.HasFile)
                {
                    Dictionary<string, object> adict = new Dictionary<string, object>();
                    DataHelper objDH = new DataHelper();
                    string sql = @"INSERT INTO [dbo].[UserUpload]
                                  ([PersonSNO]
                                  ,[CourseSNO]
                                  ,[Url]
                                  ,[Audit]
                                  ,[Note]
                                  ,[CreateUserID])
                            VALUES
                                  (@PersonSNO
                                  ,@CourseSNO
                                  ,@URL
                                  ,@Audit
                                  ,@Note
                                  ,@CreateUserID)";
                    adict.Add("PersonSNO", userInfo.PersonSNO);
                    adict.Add("CourseSNO", ddl_CourseName.SelectedValue);
                    adict.Add("URL", wPath + file_Upload.FileName);
                    adict.Add("Audit", "0");
                    adict.Add("Note", txt_Note.Text);
                    adict.Add("CreateUserID", userInfo.PersonSNO);
                    objDH.executeNonQuery(sql, adict);
                    file_Upload.SaveAs(rPath + "\\" + file_Upload.FileName);
                    DataTable adminPersonSNO = Utility.getCoursePlanningClassAdminRoleSNO(ddl_CoursePlanningClass.SelectedValue,userInfo.RoleSNO);
                    for(int i=0;i< adminPersonSNO.Rows.Count; i++)
                    {
                        
                        Utility.InsertToDO("作業上傳通知-" + userInfo.UserName + "", 
                            "已將" + ddl_CourseName.SelectedItem.Text + "作業上傳，煩請審核，請點擊" + "<a style='Color:blue' target='_blank' href='" + AuditPath + "'>審核連結</a>" + "。", 
                            adminPersonSNO.Rows[i]["PersonSNO"].ToString(), userInfo.PersonSNO, false);
                    }
                    

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('上傳成功');window.location ='FileUpload.aspx';", true);
                }
                else
                {
                    Utility.showMessage(Page, "msg", "未選取檔案!");
                }
            }
            else
            {
                errorMessage += "請上傳zip檔案或pdf檔案";
                Utility.showMessage(Page, "ErrorMessage", errorMessage);
                return;
            }
        }
        else if(Audit == "1")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('審核已通過，無須再上傳作業!');window.location ='FileUpload.aspx';", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('此課程作業已上傳，不可重複上傳，請等待管理者審閱!');window.location ='FileUpload.aspx';", true);
        }
    }

    protected void ddl_CoursePlanningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetCourse(ddl_CourseName, "請選擇");
    }

    protected void ddl_CourseName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void setCoursePlanningClassHasCtype3(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();

        string sql = "";

        sql = @"
                Select distinct QCPC.PClassSNO, QCPC.PlanName from QS_Course QC
                Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO=QC.PClassSNO
                where QC.CType=3
           
        ";

        DataTable objDT = objDH.queryData(sql, wDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }



    protected void SetCourse(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        ddl.Items.Clear();
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        wDict.Add("PClassSNO", ddl_CoursePlanningClass.SelectedValue);
        DataTable objDT = objDH.queryData("SELECT CourseSNO, CourseName FROM QS_Course where PClassSNO=@PClassSNO And Ctype=3", wDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));

        }
    }


    protected static string CheckLastCourse(string CourseSNO, string PersonSNO)
    {
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        string sql = @"Select Top(1) Audit from UserUpload where CourseSNO=@CourseSNO and PersonSNO=@PersonSNO Order By CreateDT DESC";
        DataHelper objDH = new DataHelper();
        wDict.Add("CourseSNO", CourseSNO);
        wDict.Add("PersonSNO", PersonSNO);
        DataTable ObjDT = objDH.queryData(sql, wDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT.Rows[0]["Audit"].ToString();
        }
        else
        {
            return "";
        }

    }

}