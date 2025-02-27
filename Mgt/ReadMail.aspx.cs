using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Mgt_ReadMail : System.Web.UI.Page
{
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDate();
        }
    }
    public void BindDate()
    {
        string ULSNO = Request.QueryString["ULSNO"] != null ? Request.QueryString["ULSNO"].ToString() : "";
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        if (ULSNO != "")
        {            
            string sql = "Select * from UserUpload where ULSNO=@ULSNO";
            adict.Add("ULSNO", ULSNO);
            DataTable ObjDT = ObjDH.queryData(sql, adict);
            lb_Mailcontent.Text = ObjDT.Rows[0]["MailContent"].ToString()!=""? Server.HtmlDecode(ObjDT.Rows[0]["MailContent"].ToString()):"尚未寄件";
        }
        else
        {
            lb_Mailcontent.Text = "尚未寄件";
        }
    }
}