using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;



/// <summary>
/// EmailConnection 的摘要描述
/// </summary>
/// 



[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class EmailConnection : System.Web.Services.WebService
{

    public EmailConnection()
    {

        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 
    }
    public Authentication2 authentication = new Authentication2();
    public class Authentication2 : SoapHeader
    {
        public string Token { get; set; }

    }
 
    [SoapHeader("authentication")]
    [WebMethod]
    public bool CreateGroup(string EventName)
    {
        #region 電子豹-建立群組並取得sn
         EventName = EventName + "-" + DateTime.Now.ToShortDateString();
        string sn = Email.CreateGroup(EventName);
        #endregion 

       
        return true;
    }

    public async Task<bool> InsertMemberAsync(string json,string sn)
    {
        #region 取得活動人數並且匯入
        
        //序列化為JSON字串並輸出結果
        var result = JsonConvert.SerializeObject(json, Formatting.Indented);
        await Email.InsertMember(sn, result);
        #endregion
        return true;
    }
    
    public async Task<bool> Send(string ActiveName,string sn,string MailContent)
    {
        #region 建立活動並且寄送

       await Email.SendList(ActiveName, sn, MailContent);
        #endregion
        return true;
    }

    public bool IsValidUser()
    {

        //這段依使用狀況，可以改讀資料庫的帳號密碼或Web.Conifg 等....
        if (authentication.Token == "369b4a473a61a465e47d0a9bd4300cd8d6ff83b8f348eed23aad73c9a86fa2dc")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
