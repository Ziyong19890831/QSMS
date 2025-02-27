using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_IntelAjax : System.Web.UI.Page
{
    UserInfo userInfo = null;
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Controls.Clear();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string insertPaper(string formname, string isuse, string content)
    {


        string result = "";

        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PaperName", formname);
        aDict.Add("isUse", isuse);
        aDict.Add("PaperDetail", content);
        aDict.Add("CreateUserID", 2);
        string sql = @"
            INSERT INTO Paper (
            	PaperName,
            	isUse,
            CreateUserID,
            PaperDetail,
            isWrite
            )
            VALUES
	    (
		@PaperName ,@isUse ,@CreateUserID ,@PaperDetail,'0'
	    )
            ";

        odt.executeNonQuery(sql, aDict);
        aDict.Clear();

        return result;
    }


    [WebMethod]
    public static string updatePaper(string formname, string isuse, string personid, string content, string view)
    {
        string result = "";

        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PaperName", formname);
        aDict.Add("isUse", isuse);
        aDict.Add("PaperDetail", content);
        aDict.Add("ModifyUserID", 2);
        aDict.Add("PaperID", personid);
        aDict.Add("isWrite", view);

        string sql = @"
            UPDATE Paper
            SET PaperName =@PaperName,
             isUse =@isUse,
             PaperDetail =@PaperDetail,
             ModifyDT = SYSDATETIME(),
             ModifyUserID =@ModifyUserID,
            isWrite = @isWrite
            WHERE
            	PaperID =@PaperID
            ";

        odt.executeNonQuery(sql, aDict);
        aDict.Clear();

        return result;
    }
    [WebMethod]
    public static string LogoutOver()
    {
  

            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();


        return "success";


    }
    [WebMethod]
    public static string  Logout(string para)
    {
        if (para == "False")
        {
           
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.Clear();
            //HttpContext.Current.Response.Redirect("./Notice.aspx");


        }
        else
        {
            int aa = HttpContext.Current.Session.Timeout;
            if (HttpContext.Current.Session["QSMS_UserInfo"] == null)
            {
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.Clear();
                return "fails";
            }
            else
            {
                HttpContext.Current.Session.Timeout = 30;
            }

        }
        return "success";

    }
    [WebMethod]
    public static string Login(string para)
    {
        if (para == "False")
        {

            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
            //HttpContext.Current.Response.Redirect("./Notice.aspx");


        }
        return "success";

    }
    [WebMethod]

    public static void FirstSignInELearning(UserInfo userInfo)
    {
       
        string url_course = "https://e-quitsmoking.hpa.gov.tw/qsms-api/sso/generate-url?key=UoLgyT3cLMeM9jAu0smB";
        string param = "";
        param += "firstName=" + userInfo.UserName.Substring(1);
        param += "&lastName=" + userInfo.UserName.Substring(0, 1);
        //param += "&username=" + userInfo.UserAccount;
        param += "&username=" + userInfo.PersonID;
        param += "&idNumber=" + userInfo.PersonID;
        param += "&email=" + userInfo.UserMail;
        //param += "&courseId=" + ELCode;
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

        HttpContext.Current.Response.Redirect(responseStr);
        //return "success";
    }

}