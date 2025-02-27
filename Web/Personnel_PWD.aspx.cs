using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Personnel : System.Web.UI.Page
{
    UserInfo userInfo = null;
    public string PACCOld = "";
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"]; PACCOld=userInfo.UserAccount;
        //暫時關閉
        if (userInfo == null) Response.Redirect("../Default.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
    }

    protected void btn_Update_Click(object sender, EventArgs e)
    {
        String errorMessage = "";

        if (string.IsNullOrEmpty(txt_OldPswd.Value)) errorMessage += "請輸入您的密碼！\\n";
        if (string.IsNullOrEmpty(txt_NewPWD.Value)) errorMessage += "請輸入您的新密碼！\\n";
        if (string.IsNullOrEmpty(txt_OKPWD.Value)) errorMessage += "請輸入您的確認新密碼！\\n";

        if (txt_OldPswd.Value != userInfo.UserPWD) errorMessage += "密碼不正確！\\n";  
        if (txt_NewPWD.Value != txt_OKPWD.Value)  errorMessage += "請再次確認密碼！\\n";

        if (txt_NewPWD.Value.Length > 50) errorMessage += "新密碼字數過多！\\n";
        

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonSNO", userInfo.PersonSNO);
        aDict.Add("NewPWD", txt_NewPWD.Value);
        aDict.Add("PasswordModilyDT", DateTime.Now);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Update Person Set PPWD=@NewPWD,PasswordModilyDT=@PasswordModilyDT Where PersonSNO=@PersonSNO", aDict);

        userInfo.UserPWD = txt_NewPWD.Value;
        Response.Write("<script>alert('修改成功!'); location.href='Personnel_PWD.aspx';</script>");

    }

}