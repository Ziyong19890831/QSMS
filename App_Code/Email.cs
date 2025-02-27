using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

/// <summary>
/// Email 的摘要描述
/// </summary>
public class Email
{
    public Email()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    /// <summary>
    /// 1.群組名稱
    /// </summary>
    public static string CreateGroup(string GroupName)
    {
        #region 電子豹-建立群組
        using (WebClient webClient = new WebClient())
        {
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            webClient.Headers.Add("Accept", "application/json");
            webClient.Headers.Add("x-api-key", "NDAyODIzZGY2ZGZlY2Q0NDAxNmUyMGVmODU4YTA1NzMtMTU4MzEyOTQ5Mw==");
            PostData postData = new PostData() { name = GroupName };
            string json = JsonConvert.SerializeObject(postData);
            var result = webClient.UploadString("https://api.newsleopard.com/v1/contacts/lists/insert", json);
            token Getsn = JsonConvert.DeserializeObject<token>(result);//反序列化
            string token = Getsn.sn;           
            return token;
        }
        
        #endregion
    }
    public static async Task<string> CreateGroupAsync(string GroupName)
    {
        using (WebClient webClient = new WebClient())
        {
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            webClient.Headers.Add("Accept", "application/json");
            webClient.Headers.Add("x-api-key", "NDAyODIzZGY2ZGZlY2Q0NDAxNmUyMGVmODU4YTA1NzMtMTU4MzEyOTQ5Mw==");
            PostData postData = new PostData() { name = GroupName };
            string json = JsonConvert.SerializeObject(postData);
            Task<string> task = webClient.UploadStringTaskAsync("https://api.newsleopard.com/v1/contacts/lists/insert", json);
            string content = await task;
            return content;
        }
        
    }
    /// <summary>
    /// 1.序號
    /// 2.人員清單
    /// </summary>
    public static async Task<string> InsertMember(string sn,string MemberList)
    {
        using (WebClient webClient = new WebClient())
        {
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            webClient.Headers.Add("Accept", "application/json");
            webClient.Headers.Add("x-api-key", "NDAyODIzZGY2ZGZlY2Q0NDAxNmUyMGVmODU4YTA1NzMtMTU4MzEyOTQ5Mw==");
            string URL = "https://api.newsleopard.com/v1/contacts/imports/" + sn + "/text";
            Task<string> task = webClient.UploadStringTaskAsync("https://api.newsleopard.com/v1/contacts/imports/"+sn+"/text", MemberList);
            string content = await task;
            return content;

        }
    }

    public static string InsertMemberString(string sn, string MemberList)
    {
        using (WebClient webClient = new WebClient())
        {
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            webClient.Headers.Add("Accept", "application/json");
            webClient.Headers.Add("x-api-key", "NDAyODIzZGY2ZGZlY2Q0NDAxNmUyMGVmODU4YTA1NzMtMTU4MzEyOTQ5Mw==");
            string URL = "https://api.newsleopard.com/v1/contacts/imports/" + sn + "/text";
            string task = webClient.UploadString("https://api.newsleopard.com/v1/contacts/imports/" + sn + "/text", MemberList);
            string content = task;
            return content;

        }
    }
    /// <summary>
    /// 1.活動名稱
    /// 2.序號
    /// 3.信件內容
    /// </summary>
    public static async Task<string> SendList(string EventName,string sn,string Content)
    {
        #region 建立活動且發送信件
        await Task.Delay(20000);
        //用Linq直接組
        var resultLinq = new
        {
            form = new
            {
                name = EventName,
                selectedLists = new List<string> { sn },
                excludeLists = new List<string> { "" }
            },
            config = new
            {
                schedule = new
                {
                    type = 0,
                    timezone = 21,
                    scheduleDate = DateTime.UtcNow.AddSeconds(10)
                },
                ga = new
                {
                    enable = true,
                    ecommerceEnable = false,
                    utmCampaign = "spring_sale",
                    utmContent = "logolink"

                }
            },
            content = new
            {
                subject = EventName + "信件通知",
                fromName = "醫事人員戒菸服務訓練系統-系統寄送",
                fromAddress = "hpaquitsmoking@gmail.com",
                htmlContent = "<!DOCTYPEhtml><html><head></head><body><p>" + Content + "</p></body></html>",
                footerLang = 1
            }

        };

        //序列化為JSON字串並輸出結果
        var results = JsonConvert.SerializeObject(resultLinq, Formatting.Indented);

        #endregion
        Thread.Sleep(30000);
        using (WebClient webClient = new WebClient())
        {
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            webClient.Headers.Add("Accept", "application/json");
            webClient.Headers.Add("x-api-key", "NDAyODIzZGY2ZGZlY2Q0NDAxNmUyMGVmODU4YTA1NzMtMTU4MzEyOTQ5Mw==");
            Task<string> task = webClient.UploadStringTaskAsync("https://api.newsleopard.com/v1/campaign/normal/submit", results);
            Thread.Sleep(6000);
            string content = await task;
            return content;
        }
    }

    public class PostData
    {
        public string name { get; set; }
    }

    public class token
    {
        public string sn { get; set; }
    }
}