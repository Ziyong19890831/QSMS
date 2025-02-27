using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Marquee : System.Web.UI.Page
{
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindData();
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {

        String errorMessage = "";
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        if (txt_Marquee.Text.Length>500)
        {
            errorMessage += "跑馬燈字元超過\\n";
        }
        if (txt_Marquee.Text.Length == 0)
        {
            errorMessage += "請輸入跑馬燈\\n";
        }

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        aDict.Add("Text", txt_Marquee.Text);
        aDict.Add("ModifyDT", Convert.ToDateTime(DateTime.Now));
        aDict.Add("ModifyUserID", userInfo.PersonSNO);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Update Marquee Set Text=@Text,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID Where MarqueeSNO=1", aDict);

        Response.Write("<script>alert('修改成功!');document.location.href='./Marquee.aspx'; </script>");


    }

    protected void bindData()
    {

        DataHelper objDH = new DataHelper();
        DataTable objDB = objDH.queryData(@"SELECT * FROM Marquee
                                            where MarqueeSNO=1
                                            ", null);
        txt_Marquee.Text = objDB.Rows[0]["text"].ToString();
    }
}