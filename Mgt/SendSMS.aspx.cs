using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_SendSMS : System.Web.UI.Page
{

    public UserInfo userInfo = null;
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        getSMSAsync();
    }
    public void getSMSAsync()
    {

        DataHelper objDH = new DataHelper();
        Dictionary<string, object> Dict = new Dictionary<string, object>();
        Dict.Add("SMSUsername", "SMSUsername");
        Dict.Add("SMSPsw", "SMSPsw");
        Dict.Add("SMSAPI", "SMSAPI1");
        DataTable objDT = objDH.queryData(@"
            Select 
                (Select PVal From Config Where PID=@SMSUsername) SMSUsername,
                (Select PVal From Config Where PID=@SMSPsw) SMSPsw,
                (Select PVal From Config Where PID=@SMSAPI) SMSAPI
        ", Dict);

        string api = objDT.Rows[0]["SMSAPI"].ToString();
        string username = objDT.Rows[0]["SMSUsername"].ToString();
        string password = objDT.Rows[0]["SMSPsw"].ToString();
        string Url = string.Format(api + "{0}" + "{1}", "username=" + username, "&password=" + password);
        string content;
        var request = WebRequest.Create(Url);
        var response = (HttpWebResponse)request.GetResponse();
        request.ContentType = "application/json;charset=utf-8";
        using (var sr = new StreamReader(response.GetResponseStream()))
        {
            content = sr.ReadToEnd();
        }
        SMS_point.InnerText = content.Replace("AccountPoint=","");


    }
    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
    
        string SendSmsTo = txt_phone.Text;
        
        string SMStempFile = Server.MapPath("../SMSTemp/SendSMSList.txt");
        if (!File.Exists(SMStempFile))
        {
            using (StreamWriter streamWriter = new StreamWriter(SMStempFile, true, Encoding.UTF8))
            {
                string[] Array_SendSms = SendSmsTo.Split(',');
                for (int i = 0; i < Array_SendSms.Length; i++)
                {
                    streamWriter.WriteLine("[" + 100 + i + "]");
                    streamWriter.WriteLine("dstaddr=" + Array_SendSms[i] + "");
                    streamWriter.WriteLine("smbody=" + txt_SMS.Text);
                }


            }
        }

        //Utility.sendSMS(SMStempFile);
        //Response.Write("<script>alert('維護中!'); location.href='SendSMS.aspx';</script>");
    }

}