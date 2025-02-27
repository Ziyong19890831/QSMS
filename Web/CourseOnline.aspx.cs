using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Web_CourseOnline : System.Web.UI.Page
{
    UserInfo userInfo = null;
    int viewrole = 1;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindData(1);
           
        }
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        bindData(page);
    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        //QS_CourseELearning
        String sql = @"
            SELECT 
                ROW_NUMBER() OVER (ORDER BY ce.ELName,ce.ELCode) as ROW_NO
                ,ce.ELCode ,ce.ELName ,ce.ELURL
            FROM QS_CourseELearning ce
            left join Role R on ce.PClassSNO = R.DocGroup
            Where ce.IsEnable=1
        ";
        if (Session["QSMS_UserInfo"] == null)
        {
            sql = @"
            SELECT 
                ROW_NUMBER() OVER (ORDER BY ce.ELName,ce.ELCode) as ROW_NO
                ,ce.ELCode ,ce.ELName ,ce.ELURL
            FROM QS_CourseELearning ce
            Where ce.IsEnable=1";
        }
            if (Session["QSMS_UserInfo"] != null)
        {
            if (userInfo.RoleOrganType == "S"|| userInfo.RoleOrganType == "A")
            {
                sql = @"
            SELECT 
                ROW_NUMBER() OVER (ORDER BY ce.ELName,ce.ELCode) as ROW_NO
                ,ce.ELCode ,ce.ELName ,ce.ELURL
            FROM QS_CourseELearning ce
            Where ce.IsEnable=1
        ";
            }
            else
            {
                //四個學會及四個身分
                sql += @"and R.RoleSno=@RoleGroup or ce.IsEnable=1 and R.RoleSno is null";
                wDict.Add("RoleGroup", userInfo.RoleGroup);
            }
        }
            
        DataTable objDT = objDH.queryData(sql, wDict);

        //QS_CourseELearningSection
        String sql_list = @"
            SELECT 
                ROW_NUMBER() OVER (ORDER BY ces.ELSCode) as ROW_NO
                ,ces.ELCode, ces.ELSCode ,ces.ELSName ,ces.ELSURL
            FROM QS_CourseELearningSection ces
            where ces.IsEnable=1 and ViewStatus=1
        ";
        DataTable objDT_list = objDH.queryData(sql_list, null);

        //建立兩張表的關聯
        DataSet ds = new DataSet();
        ds.Tables.Add(objDT);
        ds.Tables.Add(objDT_list);
        ds.EnforceConstraints = false;
        ds.Relations.Add("myrelation", ds.Tables[0].Columns["ELCode"], ds.Tables[1].Columns["ELCode"]);

        DataTable dt = ds.Tables[0];
        int maxPageNumber = (dt.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        dt.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        rpt_CourseOnline.DataSource = dt;
        rpt_CourseOnline.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(dt.Rows.Count, page, pageRecord);

    }

    protected void btnOpenEL_Click(object sender, EventArgs e)
    {

        if (Session["QSMS_UserInfo"] == null)
        {
            Utility.showMessage(Page, "ErrorMessage", "請先登入系統");
            return;
        }
        else
        {
            if (Utility.CheckSession(userInfo) == true)
            {
                LinkButton btn = (LinkButton)sender;
                AutoSignInELearning(btn.CommandArgument);
            }
            else
            {
                Utility.showMessage(Page, "ErrorMessage", "請至[學員資料]將您的資料填妥後，方能直接使用e-Learning！");
                return;
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }

    protected void AutoSignInELearning(string ELCode)
    {
        //string url_course = "https://e-quitsmoking.hpa.gov.tw/qsms-api/sso/generate-url?key=UoLgyT3cLMeM9jAu0smB";
        //string url_course = "https://healthtraining.elearning.hpa.gov.tw/api/sso/generate-url?key=tAfx7FaLHGz6Vd3xFR0j";
        string url_course = "https://hpaqs.mydevhost.com/qsms-api/sso/generate-url?key=UoLgyT3cLMeM9jAu0smB";
        string param = "";
        param += "firstName=" + userInfo.UserName.Substring(1);
        param += "&lastName=" + userInfo.UserName.Substring(0, 1);
        //param += "&username=" + userInfo.UserAccount;
        param += "&username=" + userInfo.PersonID;
        param += "&idNumber=" + userInfo.PersonID;
        param += "&email=" + userInfo.UserMail;
        param += "&courseId=" + ELCode;
        //firstName(必要) 名字
        //lastName(必要) 姓氏
        //username(必要) 帳號* 身分證字號*
        //email(必要) 電子郵件
        //courseId(選擇性) 課程Id，如果有填，登入後會自動導向課程頁面


        //強制認為憑證都是通過的，特殊情況再使用
        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  //因應HTTPS調整

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url_course);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";

        //要發送的字串轉為byte[] 
        byte[] byteArray = Encoding.UTF8.GetBytes(param);
        using (Stream reqStream = request.GetRequestStream())
        {
            reqStream.Write(byteArray, 0, byteArray.Length);

        }//end using

        //API回傳的字串
        string responseStr = "";
        using (WebResponse response = request.GetResponse())
        {
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseStr = sr.ReadToEnd();
            }//end using  
        }

        responseStr = responseStr.Replace("{\"loginUrl\":\"", "");
        responseStr = responseStr.Replace("\"}", "");
        responseStr = responseStr.Replace("\\", "");
        //Label1.Text = param + "<br>" + responseStr;
        string js = "window.open('" + responseStr + "', '_blank')";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "openEL", js, true);


    }
}