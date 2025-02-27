using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_AccountAjax : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
        string acc = "";
        string org = "";
        string pid = "";
        string pwd = "";
        string result = "";
        string pmail = "";
        if (Request.Form["personid"] != null)
        {
            pid = Request.Form["personid"].ToString();
        }

        if (Request.Form["account"] != null)
        {
            acc = Request.Form["account"].ToString();
        }

        if (Request.Form["orgid"] != null)
        {
            org = Request.Form["orgid"].ToString();
        }

        if (Request.Form["pwd"] != null)
        {
            pwd = Request.Form["pwd"].ToString();
        }
        if (Request.Form["pmail"] != null)
        {
            pmail = Request.Form["pmail"].ToString();
        }
        if (pid == "0")
        {
            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PAccount", acc);
            DataTable dt_acc = odt.queryData("SELECT * FROM Person WHERE PAccount=@PAccount ", aDict);
            aDict.Clear();
            

            if (dt_acc.Rows.Count != 0)
            {
                result = "您輸入的帳號已存在";

            }
            else
            {
                result = "可使用";
            }
            Response.Write(result);
            Response.End();
        }
        if (acc == "0")
        {
            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PersonID", pid);
            DataTable dt_pid = odt.queryData("SELECT * FROM Person WHERE PersonID =@PersonID", aDict);

            if (dt_pid.Rows.Count != 0)
            {
                result = "您輸入的身分證已存在";

            }
            else
            {
                result = "可使用";
            }
            Response.Write(result);
            Response.End();
        }

        if (pid == "#")
        {
            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PMail", acc);
            DataTable dt_pid = odt.queryData("SELECT * FROM Person WHERE PMail=@PMail", aDict);
            if (dt_pid.Rows.Count != 0)
            {
                result = "您輸入的信箱已存在";
            }
            else
            {
                result = "可使用";
            }
            Response.Write(result);
            Response.End();
        }
        if (pwd != "")
        {
            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PAccount", acc);
            aDict.Add("PPWD", pwd);
            DataTable dt_pwd = odt.queryData("SELECT * FROM Person WHERE PPWD collate Chinese_Taiwan_Stroke_CS_AS =@PPWD AND PAccount=@PAccount ", aDict);
            aDict.Clear();
            if (dt_pwd.Rows.Count != 0)
            {
                result = "密碼正確";
            }
            else
            {
                result = "您輸入的密碼錯誤";
            }
            Response.Write(result);
            Response.End();
        }
        if (org != "") {

            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("OrganCode", org);
            DataTable dt_org = odt.queryData("SELECT * FROM Organ WHERE OrganCode=@OrganCode", aDict);
            if (dt_org.Rows.Count != 0)
            {
                result = "可使用," + dt_org.Rows[0]["OrganSNO"] + "," + dt_org.Rows[0]["OrganCode"] + "-" + dt_org.Rows[0]["OrganName"];
            }
            else
            {
                result = "查無單位代碼";
            }
            Response.Write(result);
            Response.End();
        }
        if (pmail != "")
        {

            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PMail", pmail);
            DataTable dt_org = odt.queryData("SELECT * FROM Person WHERE PMail=@PMail", aDict);
            if (dt_org.Rows.Count == 0)
            {
                result = "可使用";
            }
            else
            {
                result = "已有重複之Mail";
            }
            Response.Write(result);
            Response.End();
        }
    }

    [WebMethod]
    public static string AutoSign(string UserName,string PersonID,string UserMail)
    {
      
        //string url_course = "https://e-quitsmoking.hpa.gov.tw/qsms-api/sso/generate-url?key=UoLgyT3cLMeM9jAu0smB";
        string url_course = "https://healthtraining.elearning.hpa.gov.tw/api/sso/generate-url?key=tAfx7FaLHGz6Vd3xFR0j";
        string param = "";
        param += "firstName=" + UserName.Substring(1);
        param += "&lastName=" + UserName.Substring(0, 1);
        param += "&username=" + PersonID;
        param += "&idNumber=" + PersonID;
        param += "&email=" + UserMail;

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

        return responseStr;
    }
}