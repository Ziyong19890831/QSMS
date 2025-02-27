using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_getPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
    }

    protected void btn_Reset_Click(object sender, EventArgs e)
    {
        GetPassword();
    }

    protected void GetPassword()
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

        bool ExistsPAccount = GetPasswordAndSendMail(txt_Acc.Value, txt_Mail.Value, txt_PersonID.Value);
        if (ExistsPAccount)
            Response.Write("<script>alert('您的密碼已寄送，請至您登記的信箱收取密碼！'); location.href='Notice.aspx';</script>");
        else
            Response.Write("<script>alert('您帳號或信箱或身分證不存在！'); </script>");

    }

    public bool GetPasswordAndSendMail(string PAccount, string PMail,string PersonID)
    {

        //確認是否有PAccount&PMail
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PAccount", PAccount);
        aDict.Add("PMail", PMail);
        aDict.Add("PersonID", PersonID);
        string sql = "Select PPWD From Person Where PAccount=@PAccount And PMail=@PMail And PersonID=@PersonID";
        DataTable objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {


            string PPWD = objDT.Rows[0]["PPWD"].ToString();
            //取得mail寄送樣板路徑，以及讀取內容
            string getTemplate = System.IO.File.ReadAllText(Server.MapPath("../SysFile/TemplateMailForGetPassword.html"));
            getTemplate = getTemplate.Replace("@RestDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            getTemplate = getTemplate.Replace("@RestNewPassword", PPWD);

            //重置密碼
          

            //發送重置密碼信件，依照各系統的發送mail配置自行調整
            Utility.SendMail("密碼取得通知信件", getTemplate, PMail);

            return true;
        }
        else
        {
            return false;
        }

    }
}