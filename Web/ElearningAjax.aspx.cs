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

public partial class Web_ElearningAjax : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["QSMS_UserInfo"] != null)
        {
            userInfo = (UserInfo)Session["QSMS_UserInfo"];
        }
        else
        {
            Response.Redirect("Notice.aspx");
        }

        //string url_course = "https://e-quitsmoking.hpa.gov.tw/qsms-api/sso/generate-url?key=UoLgyT3cLMeM9jAu0smB";
        //string url_course = "https://healthtraining.elearning.hpa.gov.tw/api/sso/generate-url?key=tAfx7FaLHGz6Vd3xFR0j";
        string url_course = "https://hpaqs.mydevhost.com/qsms-api/sso/generate-url?key=UoLgyT3cLMeM9jAu0smB";
        string param = "";
        param += "firstName=" + userInfo.UserName.Substring(1);
        param += "&lastName=" + userInfo.UserName.Substring(0, 1);
        param += "&username=" + userInfo.PersonID;
        param += "&idNumber=" + userInfo.PersonID;
        param += "&email=" + userInfo.UserMail;

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
        Response.Write(responseStr);
        Response.End();
        
    }
    
}