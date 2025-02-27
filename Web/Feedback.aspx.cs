using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Feedback : System.Web.UI.Page
{
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["QSMS_UserInfo"] != null)
        {
            if (!IsPostBack)
            {
                txt_Name.Text = userInfo.UserName;
                txt_Email.Text = userInfo.UserMail;
                txt_Phone.Text = userInfo.UserPhone;
            }
           
        }
    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {

        string errorMessage = "";

        if (string.IsNullOrEmpty(txt_Name.Text)) errorMessage += "請輸入姓名！\\n";
        if (string.IsNullOrEmpty(txt_Email.Text)) errorMessage += "請輸入Mail！\\n";
        if (string.IsNullOrEmpty(txt_Phone.Text)) errorMessage += "請輸入聯絡電話！\\n";
        if (string.IsNullOrEmpty(txt_Explain.Text)) errorMessage += "請輸入說明！\\n";

        if (txt_Name.Text.Length > 50) errorMessage += "姓名字數過多！\\n";
        if (txt_Email.Text.Length > 50) errorMessage += "Mail字數過多！\\n";
        if (txt_Phone.Text.Length > 50) errorMessage += "聯絡電話字數過多！\\n";
        if (txt_Explain.Text.Length > 500) errorMessage += "說明字數過多！\\n";

        if (txt_Verification_Right.Value != Request.Cookies["CheckCode"].Value)
        {
            Utility.showMessage(Page, "ErrorMessage", "驗證碼錯誤");
            return;
        }

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("SYSTEM_ID", "S00");
        aDict.Add("FBTYPE", rbl.Text);
        aDict.Add("Name", txt_Name.Text);
        aDict.Add("Email", txt_Email.Text);
        aDict.Add("Tel", txt_Phone.Text);
        aDict.Add("Explain", txt_Explain.Text);
       
        objDH.executeNonQuery("Insert Into Feedback(SYSTEM_ID, FBTYPE, Name, Email, Tel, Explain) Values(@SYSTEM_ID, @FBTYPE, @Name, @Email, @Tel, @Explain)", aDict);
        Response.Write("<script>alert('感謝您的回饋!'); location.href='Feedback.aspx';</script>");
    }

}