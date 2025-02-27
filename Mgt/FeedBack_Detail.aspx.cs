using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_FeedBack_Detail : System.Web.UI.Page
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
    }
    protected void binddata()
    {
        string FBSNO = Request.QueryString["sno"].ToString();
        string sql = @"SELECT [FBSNO]
                  ,[FBTYPE]
                  ,[Name]
                  ,[Rank]
                  ,[Email]
                  ,[Tel]
                  ,[Explain]
                  ,[Response]
                  ,[SYSTEM_ID]
            	  ,P.PName
                  ,FB.[CreateDT]
                  ,FB.FeedBackTitle
                  ,FB.[CreateUserID]
                  ,FB.[FeedBackContent]
                  ,FB.[FeedBackDate]
                  ,FB.[FeedBackPersonSNO]
                  ,FB.[PassTo]
                  ,FB.[ModifyDT]
                  ,FB.[ModifyUserID]
              FROM [Feedback] FB
              Left JOIN Person P ON P.PersonSNO=FB.[FeedBackPersonSNO] 
              Where FB.FBSNO=@FBSNO";
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        aDict.Add("FBSNO", FBSNO);
        DataTable objDT = ObjDH.queryData(sql, aDict);       
        lb_ReplyStutas.Text = objDT.Rows[0]["Response"].ToString();
        if(objDT.Rows[0]["PassTo"].ToString() != "")
        {
            lb_Forward.Text = objDT.Rows[0]["PassTo"].ToString();
            forward.Visible = true;
        }
        lb_ReplyTheme.Text= objDT.Rows[0]["FeedBackTitle"].ToString();
        lb_ReplyContent.Text= objDT.Rows[0]["FeedBackContent"].ToString();
        lb_FeedBackDate.Text = objDT.Rows[0]["FeedBackDate"].ToString();
        lb_ReplyPerson.Text = objDT.Rows[0]["PName"].ToString();
    }
}