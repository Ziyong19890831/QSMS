using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_ForgetPassword : System.Web.UI.Page
{

    protected void Page_Init(object sender, EventArgs e)
    {

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {

        }
    }


    protected void btn_Reset_Click(object sender, EventArgs e)
    {
        ForgetPassword();
    }

    protected void ForgetPassword()
    {

        String errorMessage = "";
        //身分證
        if (String.IsNullOrEmpty(txt_PersonID.Value)) errorMessage += "身分證不可為空白！\\n";

        //帳號
        if (String.IsNullOrEmpty(txt_Acc.Value)) errorMessage += "帳號不可為空白！\\n";

        //信箱
        if (String.IsNullOrEmpty(txt_Mail.Value)) errorMessage += "信箱不可為空白！\\n";

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        bool ExistsPAccount = RestPasswordAndSendMail(txt_Acc.Value, txt_Mail.Value,txt_PersonID.Value);
        if (ExistsPAccount)
            Response.Write("<script>alert('您的密碼已重置，請至您登記的信箱收取密碼！'); location.href='Notice.aspx';</script>");
        else
            Response.Write("<script>alert('您帳號或信箱或身分證不存在！'); </script>");

    }




    //忘記密碼＞重設＞發送Mail
    public bool RestPasswordAndSendMail(string PAccount, string PMail ,string PersonID)
    {

        //確認是否有PAccount&PMail
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PAccount", PAccount);
        aDict.Add("PMail", PMail);
        aDict.Add("PersonID", PersonID);
        string sql = "Select PersonSNO From Person Where PAccount=@PAccount And PMail=@PMail And PersonID=@PersonID";
        DataTable objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
 
            string getRandomPSW = CreateRandomCode(3, 2, 2, 1);
            aDict.Add("PPWD", getRandomPSW);
            //取得mail寄送樣板路徑，以及讀取內容
            string getTemplate = System.IO.File.ReadAllText(Server.MapPath("../SysFile/TemplateMail.html"));
            getTemplate = getTemplate.Replace("@RestDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            getTemplate = getTemplate.Replace("@RestNewPassword", getRandomPSW);

            //重置密碼
            objDH.executeNonQuery("Update Person Set PPWD=@PPWD Where PAccount=@PAccount And PMail=@PMail And PersonID=@PersonID", aDict);

            //發送重置密碼信件，依照各系統的發送mail配置自行調整
            Utility.SendMail("密碼重置通知信件", getTemplate, PMail);

            return true;
        }
        else
        {
            return false;
        }

    }


    /// <summary>
    /// 隨機取得一組字串
    /// </summary>
    /// <param name="char_m">數字出現次數</param>
    /// <param name="char_b">大寫英文出現次數</param>
    /// <param name="char_s">小寫英文出現次數</param>
    /// <param name="char_c">特殊符號出現次數</param>
    /// <returns>隨機取得一組字串</returns>
    public static string CreateRandomCode(int char_m, int char_b, int char_s, int char_c)
    {
        string allChar_m = "2,3,4,5,6,7,8,9";
        string allChar_b = "A,B,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
        string allChar_s = "a,b,c,d,e,f,g,h,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z";
        string allChar_c = "~,!,#,^,*";
        string[] allCharArray_m = allChar_m.Split(',');
        string[] allCharArray_b = allChar_b.Split(',');
        string[] allCharArray_s = allChar_s.Split(',');
        string[] allCharArray_c = allChar_c.Split(',');
        string randomCode = "";
        string returnRandom = "";
        //int temp = -1;
        Random rand = new Random();

        for (int i = 0; i < char_m; i++)
            randomCode += allCharArray_m[rand.Next(allCharArray_m.Length)];

        for (int i = 0; i < char_b; i++)
            randomCode += allCharArray_b[rand.Next(allCharArray_b.Length)];

        for (int i = 0; i < char_s; i++)
            randomCode += allCharArray_s[rand.Next(allCharArray_s.Length)];

        for (int i = 0; i < char_c; i++)
            randomCode += allCharArray_c[rand.Next(allCharArray_c.Length)];


        List<char> RandomArray = randomCode.ToCharArray().ToList();
        while (RandomArray.Count > 0)
        {
            int t = rand.Next(RandomArray.Count);
            returnRandom += RandomArray[t];
            RandomArray.RemoveAt(t);
        }

        //return randomCode + " " + returnRandom;
        return returnRandom;

    }


}