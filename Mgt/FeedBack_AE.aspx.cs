using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_FeedBack_AE : System.Web.UI.Page
{

    public UserInfo userInfo = null;
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            binddata();
           
        }

        if (RB_SendOther.Checked)
        {
            forward.Visible = true;
        }
        else
        {
            forward.Visible = false;
        }
    }

    public void binddata()
    {
        string FBSNO = Request.QueryString["sno"].ToString();
        string SQL = @"Select * from [Feedback] Where [FBSNO]=@FBSNO";
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("FBSNO", FBSNO);
        DataHelper ObjDH = new DataHelper();
        DataTable ObjDT = ObjDH.queryData(SQL, aDict);
        lb_QName.Text = ObjDT.Rows[0]["Name"].ToString();
        lb_QEmail.Text = ObjDT.Rows[0]["Email"].ToString();
        txt_Qcontent.Text = ObjDT.Rows[0]["Explain"].ToString();


    }

    
    protected void btn_Send_Click(object sender, EventArgs e)
    {
        string FBSNO = Request.QueryString["sno"].ToString();
        string SendTo = lb_QEmail.Text;
        //Utility.SendMail(txt_ReplyTheme.Text, txt_ReplyContent.Text, SendTo);
        string Update_SQL = @"Update Feedback Set Response=@Response,FeedBackTitle=@FeedBackTitle,FeedBackContent=@FeedBackContent,FeedBackDate=@FeedBackDate,FeedBackPersonSNO=@FeedBackPersonSNO,PassTo=@PassTo Where FBSNO=@FBSNO";
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper ObjDT = new DataHelper();
        aDict.Add("FBSNO", FBSNO);
        aDict.Add("FeedBackTitle", txt_ReplyTheme.Text);
        aDict.Add("FeedBackContent", editor1.Value);       
        aDict.Add("FeedBackDate", DateTime.Now);
        aDict.Add("FeedBackPersonSNO", userInfo.PersonSNO);

        if (RB_FeedBack.Checked)
        {
            aDict.Add("Response", "已回覆");
            aDict.Add("PassTo", "");
        }
        else
        {
            aDict.Add("Response", "已轉達");
            aDict.Add("PassTo", txt_Forward.Text);
        }
        ObjDT.executeNonQuery(Update_SQL, aDict);
        if (RB_FeedBack.Checked)
        { 
            Utility.SendMail(txt_ReplyTheme.Text, editor1.Value, SendTo);
        }
        else
        {
            Utility.SendMail(txt_ReplyTheme.Text, editor1.Value, txt_Forward.Text);
        }
        Response.Write("<script>opener.location.reload()</script>");
        Response.Write("<script language='javascript'>window.close();</script>");


    }
}